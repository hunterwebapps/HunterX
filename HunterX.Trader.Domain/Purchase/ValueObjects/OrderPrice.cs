namespace HunterX.Trader.Domain.Purchase.ValueObjects;

public record OrderPrice
{
    public required decimal BidPrice { get; init; }
    public required decimal AskPrice { get; init; }
    public required decimal BidSize { get; init; }
    public required decimal AskSize { get; init; }
    public required DateTime Timestamp { get; init; }
}
