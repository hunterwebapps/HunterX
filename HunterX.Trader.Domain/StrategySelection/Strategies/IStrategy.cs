using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData;

namespace HunterX.Trader.Domain.StrategySelection.Strategies;

public interface IStrategy
{
    string Name { get; }
    ExecutionDecisionDetails DetermineBuyDecision();
    ExecutionDecisionDetails DetermineSellDecision();
}
