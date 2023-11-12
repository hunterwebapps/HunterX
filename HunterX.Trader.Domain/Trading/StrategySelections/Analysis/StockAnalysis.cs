using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Trading.StrategySelections.Analysis;

internal class StockAnalysis : Entity
{
    private readonly MarketConditions marketData;

    public StockAnalysis(StockSymbol symbol, MarketConditions marketData) : base(null)
    {
        this.marketData = marketData;
    }

    public void Analyze()
    {

    }
}
