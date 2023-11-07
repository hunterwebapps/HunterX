namespace HunterX.Trader.Common.Configuration;

public class AppSettings
{
    public bool Live { get; set; }
    public AlpacaConfig Alpaca { get; set; } = default!;
    public string PolygonKey { get; set; } = default!;
    public string FMPKey { get; set; } = default!;
    public int SymbolFeedConcurrency { get; set; }
    public FinancialModelingPrepConfiguration FinancialModelingPrep { get; set; } = default!;
    public MassTransitConfig MassTransit { get; set; } = default!;
    public BackTestingConfig BackTesting { get; set; } = default!;
    public ConnectionStrings ConnectionStrings { get; set; } = default!;

}
