using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.MarketHours;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Application.Managers;

public class MarketDataManager
{
    private readonly IMarketDataService marketDataService;
    private readonly IDateTimeProvider dateTimeProvider;

    public MarketDataManager(IMarketDataService marketDataService, IDateTimeProvider dateTimeProvider)
    {
        this.marketDataService = marketDataService;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<TimeSpan> GetTimeUntilMarketOpen()
    {
        var holidays = await GetMarketHolidaysAsync(DateTime.Now);
        var marketHours = new MarketHoursRoot(this.dateTimeProvider, holidays);

        if (!marketHours.IsOffHours)
        {
            return TimeSpan.Zero;
        }

        return marketHours.GetNextMarketOpen();
    }

    public async Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync(DateTime dateTime)
    {
        var holidays = await this.marketDataService.GetMarketHolidaysAsync();

        return holidays
            .Where(h => h.Date == DateOnly.FromDateTime(dateTime))
            .ToList();
    }

    public async Task<IReadOnlyList<Asset>> GetStocksAsync()
    {
        return await this.marketDataService.GetAssetsAsync();
    }
}
