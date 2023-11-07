using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HunterX.Trader.Infrastructure.Databases.Converters;

public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (x, y) => x.DayNumber == y.DayNumber,
        dateOnly => dateOnly.GetHashCode())
    { }
}
