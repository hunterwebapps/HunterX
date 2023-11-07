namespace HunterX.Trader.Domain.StrategySelection.ValueObjects;

public record HistoricalData
{
    public required string Symbol { get; init; }
    public required DateTime Date { get; init; }
    public required decimal Open { get; init; }
    public required decimal High { get; init; }
    public required decimal Low { get; init; }
    public required decimal Close { get; init; }
    public required long Volume { get; init; }
}
