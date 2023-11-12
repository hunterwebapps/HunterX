using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Services;

public interface IDataStreamingService : IAsyncDisposable
{
    event EventHandler? Connected;
    event EventHandler? Disconnected;
    event EventHandler<Asset>? Unsubscribed;
    Task ConnectAsync();
    Task DisconnectAsync();
    Task RegisterSubscriptionHandlerAsync(Func<Bar, Task> handleBarReceived, params Asset[] stocks);
}
