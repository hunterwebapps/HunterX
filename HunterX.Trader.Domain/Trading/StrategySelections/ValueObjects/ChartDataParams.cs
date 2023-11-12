using HunterX.Trader.Domain.Common.Enums;

namespace HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

public record ChartDataParams
{
    public required string Symbol { get; init; }
    public required string TimeFrame { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}
