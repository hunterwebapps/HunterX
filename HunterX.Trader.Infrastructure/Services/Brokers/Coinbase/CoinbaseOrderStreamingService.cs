using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;

namespace HunterX.Trader.Infrastructure.Services.Brokers.Coinbase;

public class CoinbaseOrderStreamingService : IOrderStreamingService
{
    public event EventHandler? Connected;
    public event EventHandler? Disconnected;
    public event EventHandler<OrderUpdated>? OrderFilled;
    public event EventHandler<OrderUpdated>? OrderPartiallyFilled;
    public event EventHandler<string>? OrderCancelled;
    public event EventHandler<string>? OrderRejected;

    public Task ConnectAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisconnectAsync()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
