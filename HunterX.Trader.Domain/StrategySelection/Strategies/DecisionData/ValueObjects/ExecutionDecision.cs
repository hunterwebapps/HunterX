using HunterX.Trader.Domain.StrategySelection.Strategies.Enum;

namespace HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

public record ExecutionDecision
{
    public required TradeAction Action { get; init; }
    public required DateTime? SignalTime { get; set; }
    public required string Symbol { get; init; }
    public required decimal Price { get; init; }
    public required decimal Stop { get; init; }
    public required decimal Limit { get; set; }
    public required decimal StopLoss { get; set; }
}
