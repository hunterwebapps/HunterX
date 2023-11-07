using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Application.Managers;

public class MarketDataManager
{
    private readonly IMarketDataService marketDataService;

    public MarketDataManager(IMarketDataService marketDataService)
    {
        this.marketDataService = marketDataService;
    }

    public async Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync(DateTime dateTime)
    {
        var holidays = await this.marketDataService.GetMarketHolidaysAsync();

        return holidays
            .Where(h => h.Date == DateOnly.FromDateTime(dateTime))
            .ToList();
    }

    public async Task<IReadOnlyList<StockBasics>> GetStocksAsync()
    {
        return await this.marketDataService.GetStocksAsync();
    }
}
