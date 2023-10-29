namespace HunterX.Trader.Common.Configuration;

public class AppSettings
{
    public bool Live { get; set; }
    public AlpacaConfig Alpaca { get; set; } = default!;
    public string PolygonKey { get; set; } = default!;
    public string AlphaVantageKey { get; set; } = default!;
    public ConnectionStrings ConnectionStrings { get; set; } = default!;

}
