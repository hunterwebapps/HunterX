using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Domain.StrategySelection.Analysis;

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
