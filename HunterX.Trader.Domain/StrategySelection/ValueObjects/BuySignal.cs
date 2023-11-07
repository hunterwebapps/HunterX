using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Strategies.Enum;

namespace HunterX.Trader.Domain.StrategySelection.ValueObjects;

public record BuySignal
{
    public required string Symbol { get; init; }
    public required string Exchange { get; init; }
    public required TradeAction Action { get; init; }
}
