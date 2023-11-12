namespace HunterX.Trader.Common.Configuration;

public class AppSettings
{
    public bool Live { get; set; }
    public AlpacaConfig Alpaca { get; set; } = default!;
    public CoinbaseConfig Coinbase { get; set; } = default!;
    public PolygonConfig Polygon { get; set; } = default!;
    public FinancialModelingPrepConfiguration FinancialModelingPrep { get; set; } = default!;
    public MassTransitConfig MassTransit { get; set; } = default!;
    public BackTestingConfig BackTesting { get; set; } = default!;
    public ConnectionStrings ConnectionStrings { get; set; } = default!;

}
