using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.DecisionData;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Domain.StrategySelection.Analysis;

internal class StockAnalysis : Entity
{
    private readonly MarketData marketData;

    public StockAnalysis(TickerSymbol symbol, MarketData marketData) : base(null)
    {
        this.marketData = marketData;
    }

    public void Analyze()
    {

    }
}
