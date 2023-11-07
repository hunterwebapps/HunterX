namespace HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

public record ExecutionDecisionBasics
{
    public required Guid Id { get; init; }
    public string Symbol => this.ExecutionDecision.Symbol;
    public required ExecutionDecision ExecutionDecision { get; init; }
    public required string StrategyName { get; init; }
    public required DateTime CreatedAt { get; init; }
}
