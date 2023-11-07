using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HunterX.Trader.Infrastructure.Databases.Entities.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasOne(x => x.ParentOrder)
            .WithMany(x => x.Legs)
            .HasForeignKey(x => x.ParentOrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(x => x.Symbol)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired();

        builder
            .Property(x => x.StopOrderPrice)
            .HasPrecision(7, 2);

        builder
            .Property(x => x.StopLossPrice)
            .HasPrecision(7, 2);

        builder
            .Property(x => x.LimitPrice)
            .HasPrecision(7, 2);

        builder
            .Property(x => x.TrailPrice)
            .HasPrecision(7, 2);

        builder
            .Property(x => x.TrailPercent)
            .HasPrecision(3, 2);

        builder
            .Property(x => x.OrderedPrice)
            .HasPrecision(7, 2)
            .IsRequired();

        builder
            .Property(x => x.FilledPrice)
            .HasPrecision(7, 2);
    }
}
