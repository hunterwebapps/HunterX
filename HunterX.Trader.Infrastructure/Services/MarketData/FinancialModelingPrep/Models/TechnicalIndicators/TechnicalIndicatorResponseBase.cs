namespace HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep.Models.TechnicalIndicators;

public abstract class TechnicalIndicatorResponseBase
{
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
}
