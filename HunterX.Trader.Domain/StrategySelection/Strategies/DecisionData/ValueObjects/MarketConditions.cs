using HunterX.Trader.Domain.Common;

namespace HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

public class MarketConditions
{
    public string Symbol { get; set; } = default!;
    public decimal Beta { get; set; }
}
