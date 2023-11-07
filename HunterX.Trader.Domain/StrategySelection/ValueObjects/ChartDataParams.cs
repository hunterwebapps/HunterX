namespace HunterX.Trader.Domain.StrategySelection.ValueObjects;

public record ChartDataParams
{
    public required string Symbol { get; init; }
    public required string TimeFrame { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}
