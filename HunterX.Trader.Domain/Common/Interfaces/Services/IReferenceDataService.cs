using HunterX.Trader.Domain.Trading.StrategySelections.Universe.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Services;

public interface IReferenceDataService
{
    Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync();
    Task<IReadOnlyList<StockSymbol>> GetSymbolsAsync(UniverseCriteria criteria);
}
