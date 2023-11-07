using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HunterX.Trader.Infrastructure.Databases.Converters;

public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
        timeOnly => timeOnly.ToTimeSpan(),
        timeSpan => TimeOnly.FromTimeSpan(timeSpan))
    { }
}
