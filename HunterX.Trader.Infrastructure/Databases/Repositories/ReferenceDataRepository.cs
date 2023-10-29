using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HunterX.Trader.Infrastructure.Databases.Repositories;

public class ReferenceDataRepository
{
    private readonly TradingDbContext tradingDbContext;

    public ReferenceDataRepository(TradingDbContext tradingDbContext)
    {
        this.tradingDbContext = tradingDbContext;
    }

    public async Task<List<TickerSymbol>> GetTickerSymbolsAsync()
    {
        var entities = await this.tradingDbContext.TickerSymbols.ToListAsync();

        return entities
            .Select(x => new TickerSymbol(x.Ticker, x.Name, x.Exchange, Enum.Parse<MarketType>(x.Market), x.Created))
            .ToList();
    }

    public async Task InsertTickerSymbolsAsync(IEnumerable<TickerSymbol> tickerSymbols)
    {
        var entities = tickerSymbols.Select(x => new Entities.TickerSymbol()
        {
            Ticker = x.Ticker,
            Name = x.Name,
            Exchange = x.Exchange,
            Market = x.Market.ToString(),
            Created = x.Created,
        });

        await using var transaction = await this.tradingDbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);

        await this.tradingDbContext.TickerSymbols.ExecuteDeleteAsync();

        await this.tradingDbContext.TickerSymbols.AddRangeAsync(entities);

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
