using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData;

public class ExecutionDecisionDetails : Entity
{
    public ExecutionDecisionDetails(Guid? id = null) : base(id)
    { }

    public required ExecutionDecision ExecutionDecision { get; init; }
    public required Asset Stock { get; init; }
    public required string StrategyName { get; set; }
    public required IReadOnlyList<Bar> Bars { get; init; }
}
