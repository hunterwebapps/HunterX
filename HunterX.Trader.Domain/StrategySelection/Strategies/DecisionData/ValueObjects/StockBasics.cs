namespace HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

public record StockBasics
{
    public required string Symbol { get; init; }
    public required string CompanyName { get; init; }
    public required long? MarketCap { get; init; }
    public required string? Sector { get; init; }
    public required string? Industry { get; init; }
    public required decimal Beta { get; init; }
    public required decimal Price { get; init; }
    public required decimal Volume { get; init; }
    public required string Exchange { get; init; }
    public required bool IsETF { get; init; }
}
