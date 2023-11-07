using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Workers.SymbolFeed;

public class SymbolFeed : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public SymbolFeed(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = this.serviceProvider.CreateAsyncScope();

            var marketDataManager = scope.ServiceProvider.GetRequiredService<MarketDataManager>();
            var symbolFeedManager = scope.ServiceProvider.GetRequiredService<SymbolFeedManager>();
            var dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

            var easternTime = dateTimeProvider.Now;

            using var _ = Logger.AddScope("Current Time", easternTime);

            var isOffHours = easternTime.Hour < 4
                || easternTime.Hour > 20
                || easternTime.DayOfWeek == DayOfWeek.Saturday
                || easternTime.DayOfWeek == DayOfWeek.Sunday;

            if (isOffHours)
            {
                await scope.DisposeAsync();

                await WaitUntilMarketOpensAsync(easternTime, stoppingToken);
            }
            else
            {
                var holidays = await marketDataManager.GetMarketHolidaysAsync(DateTime.Now);

                if (holidays.Count > 0 && holidays.All(x => x.Closed))
                {
                    await scope.DisposeAsync();

                    await WaitUntilNextDayAsync(easternTime, holidays[0], stoppingToken);
                }
                else
                {
                    Logger.Information("Symbol Feed executing.");

                    var stocks = await marketDataManager.GetStocksAsync();

                    await symbolFeedManager.PublishSymbolFeedAsync(stocks);

                    Logger.Information("Symbol Feed published {count} symbols.", stocks.Count);

                    await scope.DisposeAsync();

                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                }
            }
        }
    }

    private async Task WaitUntilMarketOpensAsync(DateTimeOffset easternTime, CancellationToken stoppingToken)
    {
        Logger.Information("Symbol Feed not running due to off hours.");

        DateTime nextMarketOpen;

        if (easternTime.Hour >= 20
            && easternTime.DayOfWeek >= DayOfWeek.Monday
            && easternTime.DayOfWeek <= DayOfWeek.Thursday)
        {
            // If it's after 8 PM on a weekday, next open is the next day at 4 AM
            nextMarketOpen = easternTime.Date.AddDays(1).AddHours(4);
        }
        else if (
            (easternTime.Hour >= 20 && easternTime.DayOfWeek == DayOfWeek.Friday)
            || easternTime.DayOfWeek == DayOfWeek.Saturday
            || easternTime.DayOfWeek == DayOfWeek.Sunday)
        {
            // If its after 8 PM on a weekend, next open is Monday at 4 AM
            var daysUntilMonday = ((int)DayOfWeek.Monday - (int)easternTime.DayOfWeek + 7) % 7;
            nextMarketOpen = easternTime.Date.AddDays(daysUntilMonday).AddHours(4);
        }
        else
        {
            // Otherwise, next open is 4 AM today
            nextMarketOpen = easternTime.Date.AddHours(4);
        }

        var timeUntilMarketOpen = nextMarketOpen - easternTime;

        Logger.Information("Waiting until market opens in {timeUntilMarketOpen}.", timeUntilMarketOpen);

        await Task.Delay(timeUntilMarketOpen, stoppingToken);
    }

    private async Task WaitUntilNextDayAsync(DateTimeOffset easternTime, MarketHoliday holiday, CancellationToken stoppingToken)
    {
        Logger.Information("Symbol Feed not running due to Market Holiday, {HolidayName}.", holiday.Name);

        var tomorrowMarketOpen = easternTime.Date.AddDays(1).AddHours(4);

        var timeUntilMarketOpen = tomorrowMarketOpen - easternTime;

        Logger.Information("Waiting until market opens in {timeUntilMarketOpen}.", timeUntilMarketOpen);

        await Task.Delay(timeUntilMarketOpen, stoppingToken);
    }
}
