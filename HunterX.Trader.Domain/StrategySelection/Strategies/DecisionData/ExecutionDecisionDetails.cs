using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData;

public class ExecutionDecisionDetails : Entity
{
    public ExecutionDecisionDetails(Guid? id = null) : base(id)
    { }

    public required ExecutionDecision ExecutionDecision { get; init; }
    public required StockBasics StockBasics { get; init; }
    public required string StrategyName { get; set; }
    public required IReadOnlyList<ChartData> Bars { get; init; }
    public required TechnicalIndicators Indicators { get; set; }
    public required DateTime CreatedAt { get; init; }
}
