namespace HunterX.Trader.Infrastructure.Services.ReferenceData.FinancialModelingPrep.Models;

public class StockResponse
{
    public string Symbol { get; set; } = default!;
    public string CompanyName { get; set; } = default!;
    public long MarketCap { get; set; }
    public string Sector { get; set; } = default!;
    public string Industry { get; set; } = default!;
    public decimal Beta { get; set; }
    public decimal Price { get; set; }
    public decimal LastAnnualDividend { get; set; }
    public decimal Volume { get; set; }
    public string Exchange { get; set; } = default!;
    public string ExchangeShortName { get; set; } = default!;
    public string Country { get; set; } = default!;
    public bool IsETF { get; set; }
    public bool IsActivelyTrading { get; set; }
}
