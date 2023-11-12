using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Services;

public interface ISymbolFeedService
{
    Task SubmitSymbolsForProcessingAsync(IEnumerable<Asset> stocks);
}
