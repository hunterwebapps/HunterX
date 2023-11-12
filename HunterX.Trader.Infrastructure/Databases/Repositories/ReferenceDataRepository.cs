using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HunterX.Trader.Infrastructure.Databases.Repositories;

public class ReferenceDataRepository
{
    private readonly TradingSqlDbContext tradingDbContext;

    public ReferenceDataRepository(TradingSqlDbContext tradingDbContext)
    {
        this.tradingDbContext = tradingDbContext;
    }

    public async Task<List<StockSymbol>> GetStockSymbolsAsync()
    {
        var entities = await this.tradingDbContext.StockSymbols.ToListAsync();

        return entities
            .Select(x => new StockSymbol()
            {
                Symbol = x.Symbol,
                Name = x.Name,
                Exchange = x.Exchange,
                Market = Enum.Parse<MarketType>(x.Market),
                Created = x.Created,
            })
            .ToList();
    }

    public async Task InsertStockSymbolsAsync(IEnumerable<StockSymbol> stockSymbols)
    {
        var entities = stockSymbols.Select(x => new Entities.StockSymbol()
        {
            Symbol = x.Symbol,
            Name = x.Name,
            Exchange = x.Exchange,
            Market = x.Market.ToString(),
            Created = x.Created,
        });

        await using var transaction = await this.tradingDbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);

        await this.tradingDbContext.StockSymbols.ExecuteDeleteAsync();

        await this.tradingDbContext.StockSymbols.AddRangeAsync(entities);

        await this.tradingDbContext.SaveChangesAsync();

        await transaction.CommitAsync();
    }

    public async Task<List<MarketHoliday>> GetMarketHolidaysAsync()
    {
        var entities = await this.tradingDbContext.MarketHolidays.ToListAsync();

        return entities
            .Select(x => new MarketHoliday(x.Date, x.Open, x.Close, x.Exchange, x.Name, x.Created))
            .ToList();
    }

    public async Task InsertMarketHolidaysAsync(IEnumerable<MarketHoliday> marketHolidays)
    {
        var entities = marketHolidays.Select(x => new Entities.MarketHoliday()
        {
            Date = x.Date,
            Open = x.Open,
            Close = x.Close,
            Exchange = x.Exchange,
            Name = x.Name,
            Created = x.Created,
        });

        await using var transaction = await this.tradingDbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);

        await this.tradingDbContext.MarketHolidays.ExecuteDeleteAsync();

        await this.tradingDbContext.MarketHolidays.AddRangeAsync(entities);

        await this.tradingDbContext.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}
