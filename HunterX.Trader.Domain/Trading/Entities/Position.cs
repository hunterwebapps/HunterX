using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.Trading.Enums;

namespace HunterX.Trader.Domain.Trading.ValueObjects;

public class Position : Entity
{
    public Position(Guid id) : base(id)
    { }

    public required string Symbol { get; init; }
    public required AssetClass AssetClass { get; init; }
    public required PositionSide Side { get; init; }
    public required decimal AverageEntryPrice { get; init; }
    public required int Quantity { get; init; }
    public required decimal? MarketValue { get; init; }
}
