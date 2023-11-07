using HunterX.Trader.Infrastructure.Databases.Entities.Enums;

namespace HunterX.Trader.Infrastructure.Databases.Entities;

public class Order
{
    public required Guid OrderId { get; init; }
    public required Guid? ParentOrderId { get; init; }
    public virtual Order? ParentOrder { get; init; }
    public required string Symbol { get; init; }
    public required int Quantity { get; init; }
    public required OrderSide OrderSide { get; init; }
    public required AssetClass AssetClass { get; init; }
    public required OrderType OrderType { get; init; }
    public required TimeInForce TimeInForce { get; init; }
    public required decimal? StopOrderPrice { get; init; }
    public required decimal? StopLossPrice { get; init; }
    public required decimal? LimitPrice { get; init; }
    public required decimal? TrailPrice { get; init; }
    public required decimal? TrailPercent { get; init; }
    public required decimal OrderedPrice { get; init; }
    public required decimal? FilledPrice { get; set; }
    public required virtual List<Order> Legs { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? FilledAt { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? FailedAt { get; set; }
}
