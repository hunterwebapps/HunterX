using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HunterX.Trader.Infrastructure.Databases.Entities.Configuration;

public class MarketHolidayConfiguration : IEntityTypeConfiguration<MarketHoliday>
{
    public void Configure(EntityTypeBuilder<MarketHoliday> builder)
    {
        builder.HasKey(x => new { x.Date, x.Exchange });

        builder
            .Property(x => x.Exchange)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();
    }
}
