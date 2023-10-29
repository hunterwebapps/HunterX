namespace HunterX.Trader.Infrastructure.Databases.Entities;

public class TickerSymbol
{
    public string Ticker { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Exchange { get; set; } = default!;
    public string Market { get; set; } = default!;
    public DateTime Created { get; set; }
}
