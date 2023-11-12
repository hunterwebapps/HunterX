using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.MarketHours;

public class MarketHoursRoot : AggregateRoot
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IReadOnlyList<MarketHoliday> marketHolidays;

    public MarketHoursRoot(IDateTimeProvider dateTimeProvider, IReadOnlyList<MarketHoliday> marketHolidays)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.marketHolidays = marketHolidays;
    }

    public bool IsOffHours =>
        this.dateTimeProvider.Now.Hour< 4
        || this.dateTimeProvider.Now.Hour> 20
        || this.dateTimeProvider.Now.DayOfWeek == DayOfWeek.Saturday
        || this.dateTimeProvider.Now.DayOfWeek == DayOfWeek.Sunday;

    public TimeSpan GetNextMarketOpen()
    {
        DateTime easternTime = this.dateTimeProvider.Now.DateTime;
        DateTime nextMarketOpen;

        if (this.marketHolidays.Any(x => x.Date == DateOnly.FromDateTime(this.dateTimeProvider.Now.DateTime)))
        {
            easternTime = easternTime.AddDays(1).Date;
        }
        
        if (easternTime.Hour >= 20
            && easternTime.DayOfWeek >= DayOfWeek.Monday
            && easternTime.DayOfWeek <= DayOfWeek.Thursday)
        {
            // If it's after 8 PM on a weekday, next open is the next day at 4 AM
            nextMarketOpen = easternTime.Date.AddDays(1).AddHours(4);
        }
        else if (
            easternTime.Hour >= 20 && easternTime.DayOfWeek == DayOfWeek.Friday
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

        return timeUntilMarketOpen;
    }
}
