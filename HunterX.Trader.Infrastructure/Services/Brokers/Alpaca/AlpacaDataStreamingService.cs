using Alpaca.Markets;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Infrastructure.Services.Brokers.Alpaca;

public class AlpacaDataStreamingService : IDataStreamingService
{
    private readonly IAlpacaDataStreamingClient dataClient;

    public event EventHandler? Connected;
    public event EventHandler? Disconnected;
    public event EventHandler<Asset>? Unsubscribed;

    public AlpacaDataStreamingService(AppSettings appSettings)
    {
        var secretKey = new SecretKey(appSettings.Alpaca.Key, appSettings.Alpaca.Secret);

        var environment = appSettings.Live
            ? Environments.Live
            : Environments.Paper;

        this.dataClient = environment.GetAlpacaDataStreamingClient(secretKey);

        this.dataClient.Connected += HandleConnected;
        this.dataClient.SocketClosed += HandleDisconnected;
        this.dataClient.OnError += HandleError;

        Logger.Information("Alpaca Broker Service Initialized with Target {Url}", environment.AlpacaTradingApi);
    }

    public async Task ConnectAsync()
    {
        var authStatus = await this.dataClient.ConnectAndAuthenticateAsync();

        if (authStatus != AuthStatus.Authorized)
        {
            throw new Exception($"Failed to Authenticate with Alpaca Data Streaming Client: {authStatus}");
        }

        Logger.Information("Successfully Authenticated with Alpaca Data Streaming Client");
    }

    public async Task RegisterSubscriptionHandlerAsync(Func<Bar, Task> handleBarReceived, params Asset[] stocks)
    {
        var subscriptions = new List<IAlpacaDataSubscription>();

        foreach (var stock in stocks)
        {
            var subscription = this.dataClient.GetMinuteBarSubscription(stock.Symbol);

            if (subscription.Subscribed)
            {
                Logger.Warning("Already Subscribed to {Symbol}", stock.Symbol);
                continue;
            }

            subscription.Received += async (bar) =>
            {
                Logger.Information("Received Bar from Alpaca API {@Bar}", bar);

                await handleBarReceived(new Bar()
                {
                    Symbol = bar.Symbol,
                    Date = bar.TimeUtc,
                    Open = bar.Open,
                    High = bar.High,
                    Low = bar.Low,
                    Close = bar.Close,
                    Volume = bar.Volume,
                    VWAP = bar.Vwap,
                });
            };

            subscription.OnSubscribedChanged += () =>
            {
                if (!subscription.Subscribed)
                {
                    this.Unsubscribed?.Invoke(this, stock);
                }
            };

            subscriptions.Add(subscription);
        }

        if (subscriptions.Count == 0)
        {
            return;
        }

        await this.dataClient.SubscribeAsync(subscriptions);
    }

    public async Task DisconnectAsync()
    {
        await this.dataClient.DisconnectAsync();
    }

    private void HandleConnected(AuthStatus status)
    {
        this.Connected?.Invoke(this, EventArgs.Empty);
    }

    private void HandleDisconnected()
    {
        this.Disconnected?.Invoke(this, EventArgs.Empty);
    }

    private void HandleError(Exception ex)
    {
        Logger.Error(ex, "Alpaca Data Streaming Error");
    }

    public async ValueTask DisposeAsync()
    {
        await this.dataClient.DisconnectAsync();

        this.dataClient.Dispose();
    }
}
