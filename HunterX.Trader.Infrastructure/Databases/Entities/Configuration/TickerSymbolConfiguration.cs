using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HunterX.Trader.Infrastructure.Databases.Entities.Configuration;

public class TickerSymbolConfiguration : IEntityTypeConfiguration<TickerSymbol>
{
    public void Configure(EntityTypeBuilder<TickerSymbol> builder)
    {
        builder
            .Property(x => x.Ticker)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Exchange)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.Market)
            .HasMaxLength(50)
            .IsRequired();
    }
}
