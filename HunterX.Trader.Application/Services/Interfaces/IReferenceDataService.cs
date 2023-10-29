using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Application.Services.Interfaces;

public interface IReferenceDataService
{
    Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync();
    Task<IReadOnlyList<TickerSymbol>> GetSymbolsAsync(UniverseCriteria criteria);
}
