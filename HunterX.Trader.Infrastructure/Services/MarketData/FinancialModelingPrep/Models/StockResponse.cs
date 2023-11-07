namespace HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep.Models;

public class StockResponse
{
    public string? Symbol { get; set; }
    public string? CompanyName { get; set; }
    public long? MarketCap { get; set; }
    public string? Sector { get; set; }
    public string? Industry { get; set; }
    public decimal? Beta { get; set; }
    public decimal? Price { get; set; }
    public decimal? LastAnnualDividend { get; set; }
    public decimal? Volume { get; set; }
    public string? Exchange { get; set; }
    public string? ExchangeShortName { get; set; }
    public string? Country { get; set; }
    public bool? IsETF { get; set; }
    public bool? IsActivelyTrading { get; set; }
}
