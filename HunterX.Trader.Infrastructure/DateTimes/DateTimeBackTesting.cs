using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Domain.Common.Interfaces;

namespace HunterX.Trader.Infrastructure.DateTimes;

public class DateTimeBackTesting : IDateTimeProvider
{
    private readonly static TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public DateTimeBackTesting(AppSettings appSettings)
    {
        if (!appSettings.BackTesting.Enabled)
        {
            throw new InvalidOperationException("Back testing is not enabled");
        }

        this.Now = new DateTimeOffset(appSettings.BackTesting.ESTNow, timeZoneInfo.BaseUtcOffset);
    }

    public DateTimeOffset Now { get; }
}
