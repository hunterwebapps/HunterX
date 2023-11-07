using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Application.Managers;

public class SymbolFeedManager
{
    private readonly ISymbolFeedService symbolFeedService;

    public SymbolFeedManager(ISymbolFeedService symbolFeedService)
    {
        this.symbolFeedService = symbolFeedService;
    }

    public async Task PublishSymbolFeedAsync(IEnumerable<StockBasics> stocks)
    {
        await this.symbolFeedService.SubmitSymbolsForProcessingAsync(stocks);
    }
}
