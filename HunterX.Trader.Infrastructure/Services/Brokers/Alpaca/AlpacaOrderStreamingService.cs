using Alpaca.Markets;
using Alpaca.Markets.Extensions;
using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Purchase.ValueObjects;
using System.Net.WebSockets;

namespace HunterX.Trader.Infrastructure.Services.Brokers.Alpaca;

public sealed class AlpacaOrderStreamingService : IOrderStreamingService, IDisposable
{
    private readonly IAlpacaStreamingClient streamingClient;

    public event EventHandler? Connected;
    public event EventHandler? Disconnected;

    public event EventHandler<OrderUpdated>? OrderFilled;
    public event EventHandler<OrderUpdated>? OrderPartiallyFilled;
    public event EventHandler<string>? OrderCancelled;
    public event EventHandler<string>? OrderRejected;

    public event EventHandler<Exception>? ErrorOccurred;

    public AlpacaOrderStreamingService(AppSettings appSettings)
    {
        var secretKey = new SecretKey(appSettings.Alpaca.Key, appSettings.Alpaca.Secret);

        var environment = appSettings.Live
            ? Environments.Live
            : Environments.Paper;

        var reconnectionParams = new ReconnectionParameters()
        {
            MaxReconnectionAttempts = 10,
            MaxReconnectionDelay = TimeSpan.FromMinutes(5),
            MinReconnectionDelay = TimeSpan.FromSeconds(5),
        };

        reconnectionParams.RetryWebSocketErrorCodes.Add(WebSocketError.NativeError);
        reconnectionParams.RetryWebSocketErrorCodes.Add(WebSocketError.Faulted);
        reconnectionParams.RetryWebSocketErrorCodes.Add(WebSocketError.HeaderError);

        this.streamingClient = environment
            .GetAlpacaStreamingClient(secretKey)
            .WithReconnect(reconnectionParams);

        this.streamingClient.OnError += HandleError;
        this.streamingClient.OnTradeUpdate += HandleTradeUpdated;
        this.streamingClient.OnWarning += HandleWarning;
        this.streamingClient.SocketOpened += HandleConnected;
        this.streamingClient.SocketClosed += HandleDisconnected;
    }

    public async Task ConnectAsync()
    {
        await this.streamingClient.ConnectAndAuthenticateAsync();
    }

    public async Task DisconnectAsync()
    {
        await this.streamingClient.DisconnectAsync();
    }

    public void Dispose()
    {
        this.streamingClient.Dispose();
    }

    private void HandleTradeUpdated(ITradeUpdate tradeUpdate)
    {
        switch (tradeUpdate.Event)
        {
            case TradeEvent.Fill:
                this.OrderFilled?.Invoke(this, new OrderUpdated()
                {
                    Symbol = tradeUpdate.Order.Symbol,
                    Position = (int)tradeUpdate.PositionIntegerQuantity!.Value,
                    FilledPrice = tradeUpdate.Price!.Value,
                    CancelledAt = tradeUpdate.Order.CancelledAtUtc,
                    ExpiredAt = tradeUpdate.Order.ExpiredAtUtc,
                    FailedAt = tradeUpdate.Order.FailedAtUtc,
                    FilledAt = tradeUpdate.Order.FilledAtUtc,
                    UpdatedAt = tradeUpdate.Order.UpdatedAtUtc!.Value,
                    OrderId = tradeUpdate.Order.OrderId,
                });
                break;
            case TradeEvent.PartialFill:
            case TradeEvent.PartiallyFilled:
                this.OrderPartiallyFilled?.Invoke(this, new OrderUpdated()
                {
                    Symbol = tradeUpdate.Order.Symbol,
                    Position = (int)tradeUpdate.PositionIntegerQuantity!.Value,
                    FilledPrice = tradeUpdate.Price!.Value,
                    CancelledAt = tradeUpdate.Order.CancelledAtUtc,
                    ExpiredAt = tradeUpdate.Order.ExpiredAtUtc,
                    FailedAt = tradeUpdate.Order.FailedAtUtc,
                    FilledAt = tradeUpdate.Order.FilledAtUtc,
                    UpdatedAt = tradeUpdate.Order.UpdatedAtUtc!.Value,
                    OrderId = tradeUpdate.Order.OrderId,
                });
                break;
            case TradeEvent.Rejected:
                this.OrderRejected?.Invoke(this, tradeUpdate.Order.Symbol);
                break;
            case TradeEvent.Canceled:
                this.OrderCancelled?.Invoke(this, tradeUpdate.Order.Symbol);
                break;
        }
    }

    private void HandleConnected()
    {
        Logger.Information($"{nameof(AlpacaOrderStreamingService)} Socket Opened.");
        this.Connected?.Invoke(this, EventArgs.Empty);
    }

    private void HandleDisconnected()
    {
        Logger.Information($"{nameof(AlpacaOrderStreamingService)} Socket Closed.");
        this.Disconnected?.Invoke(this, EventArgs.Empty);
    }

    private void HandleError(Exception exception)
    {
        Logger.Error(exception, $"{nameof(AlpacaOrderStreamingService)} Errored.");
        this.ErrorOccurred?.Invoke(this, exception);
    }

    private void HandleWarning(string warning)
    {
        Logger.Warning($"{nameof(AlpacaOrderStreamingService)} Warning: {warning}", warning);
    }
}
