using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.Trading.Purchases.Enums;

namespace HunterX.Trader.Domain.Trading.Purchases.ValueObjects;

public class Order
{
    public required Guid OrderId { get; init; }
    public required string Symbol { get; init; }
    public required int Quantity { get; init; }
    public required OrderSide OrderSide { get; init; }
    public required AssetClass AssetClass { get; init; }
    public required OrderType OrderType { get; init; }
    public required TimeInForce TimeInForce { get; init; }
    public required decimal? StopOrderPrice { get; init; }
    public required decimal? LimitPrice { get; set; }
    public required decimal? TrailPrice { get; set; }
    public required decimal? TrailPercent { get; set; }
    public required decimal? FilledPrice { get; init; }
    public required virtual List<Order> Legs { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime? UpdatedAt { get; set; }
    public required DateTime? FilledAt { get; set; }
    public required DateTime? ExpiredAt { get; set; }
    public required DateTime? CancelledAt { get; set; }
    public required DateTime? FailedAt { get; set; }
}
