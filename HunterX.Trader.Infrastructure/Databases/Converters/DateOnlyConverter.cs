using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HunterX.Trader.Infrastructure.Databases.Converters;

internal class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter()
        : base(d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
    { }
}
