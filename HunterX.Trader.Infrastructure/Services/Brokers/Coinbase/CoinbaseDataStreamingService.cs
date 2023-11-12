using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Infrastructure.Services.Brokers.Coinbase;

public class CoinbaseDataStreamingService : IDataStreamingService
{
    public event EventHandler? Connected;
    public event EventHandler? Disconnected;
    public event EventHandler<Asset>? Unsubscribed;

    public Task ConnectAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisconnectAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public Task RegisterSubscriptionHandlerAsync(Func<Bar, Task> handleBarReceived, params Asset[] stocks)
    {
        throw new NotImplementedException();
    }
}
