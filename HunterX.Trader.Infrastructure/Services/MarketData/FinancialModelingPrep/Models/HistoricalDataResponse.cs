namespace HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep.Models;

public class HistoricalDataResponse
{
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal Close { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public int Volume { get; set; }
}
