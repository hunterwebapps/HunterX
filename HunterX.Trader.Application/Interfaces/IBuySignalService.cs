using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Application.Interfaces;

public interface IBuySignalService
{
    Task SubmitBuySignalsAsync(IEnumerable<ExecutionDecision> buySignals);
}
