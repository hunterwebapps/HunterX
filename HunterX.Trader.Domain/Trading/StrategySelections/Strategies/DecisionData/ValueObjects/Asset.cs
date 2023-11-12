namespace HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;

public record Asset
{
    public required string Symbol { get; init; }
    public required string CompanyName { get; init; }
}
