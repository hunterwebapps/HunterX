using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Infrastructure.Databases.Converters;
using HunterX.Trader.Infrastructure.Databases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HunterX.Trader.Infrastructure.Databases;

public class TradingSqlDbContext : DbContext
{
    private readonly string connectionString;
    public TradingSqlDbContext(AppSettings appSettings, DbContextOptions<TradingSqlDbContext> options)
        : base(options)
    {
        this.connectionString = appSettings.ConnectionStrings.TradingDb;
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<MarketHoliday> MarketHolidays { get; set; }
    public DbSet<StockSymbol> StockSymbols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(this.connectionString);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v));

        configurationBuilder
            .Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
            .HaveColumnType("date");

        configurationBuilder
            .Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter, TimeOnlyComparer>()
            .HaveColumnType("time");

        configurationBuilder
            .Properties<Enum>()
            .HaveConversion<string>()
            .HaveColumnType("varchar(50)");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingSqlDbContext).Assembly);
    }
}
