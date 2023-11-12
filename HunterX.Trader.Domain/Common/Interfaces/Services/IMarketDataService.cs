using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Services;

public interface IMarketDataService
{
    Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync();
    Task<IReadOnlyList<Asset>> GetAssetsAsync();
    Task<IReadOnlyList<Asset>> GetAssetsAsync(IEnumerable<string> symbols);
    Task<IReadOnlyList<Bar>> GetChartDataAsync(ChartDataParams parameters);
    Task<IReadOnlyList<OrderPrice>> GetOrderPricesAsync(string symbol, DateTime from, DateTime to);
}
