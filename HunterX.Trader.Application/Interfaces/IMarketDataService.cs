using HunterX.Trader.Domain.Purchase.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Application.Interfaces;

public interface IMarketDataService
{
    Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync();
    Task<IReadOnlyList<StockBasics>> GetStocksAsync();
    Task<IReadOnlyList<ChartData>> GetChartDataAsync(ChartDataParams parameters);
    Task<IReadOnlyList<OrderPrice>> GetOrderPricesAsync(string symbol, DateTime from, DateTime to);
}
