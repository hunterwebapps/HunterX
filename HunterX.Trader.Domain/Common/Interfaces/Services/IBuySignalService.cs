using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Services;

public interface IBuySignalService
{
    Task SubmitBuySignalsAsync(IEnumerable<ExecutionDecision> buySignals);
}
