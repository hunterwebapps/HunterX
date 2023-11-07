namespace HunterX.Trader.Common.Configuration;

public class FinancialModelingPrepConfiguration
{
    public string Key { get; set; } = default!;
    public int RequestsPerMinute { get; set; }
    public int RequestsPerSecond { get; set; }
}
