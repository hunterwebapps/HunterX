using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.Enum;

namespace HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

public record BuySignal
{
    public required string Symbol { get; init; }
    public required string Exchange { get; init; }
    public required TradeAction Action { get; init; }
}
