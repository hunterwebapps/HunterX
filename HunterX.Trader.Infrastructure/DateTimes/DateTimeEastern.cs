using HunterX.Trader.Domain.Common.Interfaces;

namespace HunterX.Trader.Infrastructure.DateTimes;

public class DateTimeEastern : IDateTimeProvider
{
    private readonly static TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public DateTimeEastern()
    {
        this.Now = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, timeZoneInfo);
    }

    public DateTimeOffset Now { get; }
}
