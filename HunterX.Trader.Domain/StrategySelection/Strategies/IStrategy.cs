using HunterX.Trader.Domain.StrategySelection.Strategies.ValueObjects;

namespace HunterX.Trader.Domain.StrategySelection.Strategies;

public interface IStrategy
{
    ExecutionDecision DetermineExecutionDecision();
}
