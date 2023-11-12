using Skender.Stock.Indicators;

namespace HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

public record Bar : IQuote
{
    public required string Symbol { get; set; }
    public required DateTime Date { get; set; }
    public required decimal Open { get; set; }
    public required decimal Close { get; set; }
    public required decimal High { get; set; }
    public required decimal Low { get; set; }
    public required decimal Volume { get; set; }
    public decimal? VWAP { get; set; }
}
