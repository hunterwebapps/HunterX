using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Infrastructure.Services.MarketData;

public class CryptoMarketDataService : IMarketDataService
{
    public Task<IReadOnlyList<Asset>> GetAssetsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Asset>> GetAssetsAsync(IEnumerable<string> symbols)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Bar>> GetChartDataAsync(ChartDataParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<OrderPrice>> GetOrderPricesAsync(string symbol, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }
}
