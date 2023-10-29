using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Infrastructure.Databases.Entities;
using Microsoft.EntityFrameworkCore;

namespace HunterX.Trader.Infrastructure.Databases;

public class TradingDbContext : DbContext
{
    private readonly string connectionString;
    public TradingDbContext(AppSettings appSettings, DbContextOptions<TradingDbContext> options)
        : base(options)
    {
        this.connectionString = appSettings.ConnectionStrings.TradingDb;
    }

    public DbSet<MarketHoliday> MarketHolidays { get; set; } = null!;
    public DbSet<TickerSymbol> TickerSymbols { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(this.connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingDbContext).Assembly);
    }
}
