namespace HunterX.Trader.Domain.Trading.Purchases.ValueObjects;

public record OrderUpdated
{
    public required Guid OrderId { get; init; }
    public required string Symbol { get; init; }
    public required decimal? FilledPrice { get; init; }
    public required DateTime? FilledAt { get; init; }
    public required DateTime? UpdatedAt { get; init; }
    public required DateTime? ExpiredAt { get; init; }
    public required DateTime? CancelledAt { get; init; }
    public required DateTime? FailedAt { get; init; }
}
