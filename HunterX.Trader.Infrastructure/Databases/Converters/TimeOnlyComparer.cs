using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HunterX.Trader.Infrastructure.Databases.Converters;

public class TimeOnlyComparer : ValueComparer<TimeOnly>
{
    public TimeOnlyComparer() : base(
        (x, y) => x.Ticks == y.Ticks,
        timeOnly => timeOnly.GetHashCode())
    { }
}
