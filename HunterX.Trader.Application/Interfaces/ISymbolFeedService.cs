using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Application.Interfaces;

public interface ISymbolFeedService
{
    Task SubmitSymbolsForProcessingAsync(IEnumerable<StockBasics> stocks);
}
