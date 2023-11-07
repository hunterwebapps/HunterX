using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HunterX.Trader.Infrastructure.Databases.Entities.Configuration;

public class StockSymbolConfiguration : IEntityTypeConfiguration<StockSymbol>
{
    public void Configure(EntityTypeBuilder<StockSymbol> builder)
    {
        builder.HasKey(x => new { x.Symbol, x.Exchange });

        builder
            .Property(x => x.Symbol)
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
