using HunterX.Trader.Domain.StrategySelection.Universe.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Application.Interfaces;

public interface IReferenceDataService
{
    Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync();
    Task<IReadOnlyList<StockSymbol>> GetSymbolsAsync(UniverseCriteria criteria);
}
