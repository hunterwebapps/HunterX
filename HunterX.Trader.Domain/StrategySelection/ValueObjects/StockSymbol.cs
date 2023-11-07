using HunterX.Trader.Domain.Common.Enums;

namespace HunterX.Trader.Domain.StrategySelection.ValueObjects;

public record StockSymbol
{
    public required string Symbol { get; init; }
    public required string Name { get; init; }
    public required string Exchange { get; init; }
    public required MarketType Market { get; init; }
    public required DateTime Created { get; init; }
}
      