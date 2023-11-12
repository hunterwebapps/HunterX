using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Trading.StrategySelections;

public class StrategySelection : Entity
{
    private readonly Asset stock;
    private readonly IReadOnlyList<Bar> bars;

    public StrategySelection(Asset stock, IReadOnlyList<Bar> bars) : base(null)
    {
        this.stock = stock;
        this.bars = bars;
    }

    public IStrategy? DetermineBestStrategy()
    {
        IStrategy? mostViableStrategy = null;
        decimal highestViabilityRate = 0;

        var trendStrategy = new TrendFollowing(this.stock, this.bars);

        var viabilityRate = trendStrategy.DetermineViabilityRate();

        if (viabilityRate > highestViabilityRate)
        {
            highestViabilityRate = viabilityRate;
            mostViableStrategy = trendStrategy;
        }

        if (highestViabilityRate < 0.75m)
        {
            return null;
        }

        return mostViableStrategy;
    }
}
