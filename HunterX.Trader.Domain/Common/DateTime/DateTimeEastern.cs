using HunterX.Trader.Domain.Common.Interfaces;

namespace HunterX.Trader.Domain.Common.DateTime;

public readonly struct DateTimeEastern : IDateTimeProvider
{
    private readonly static TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public DateTimeEastern()
    {
        this.Now = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, timeZoneInfo);
    }

    public DateTimeEastern(System.DateTime easternTime)
    {
        this.Now = new DateTimeOffset(easternTime, timeZoneInfo.BaseUtcOffset);
    }

    public DateTimeOffset Now { get; }
}
