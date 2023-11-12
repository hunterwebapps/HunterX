using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Trading.StrategySelections.Strategies;

public interface IStrategy
{
    string Name { get; }
    IReadOnlyList<Bar> Bars { get; }
    Asset Stock { get; }
    decimal DetermineViabilityRate();
    ExecutionDecision EvaluateLatestBar(Bar bar);
}
