using HunterX.Trader.Application.Services.Interfaces;

namespace HunterX.Trader.Application.Managers;

public class AnalysisManager
{
    private readonly IReferenceDataService referenceDataService;
    private readonly IMarketDataService marketDataService;

    public AnalysisManager(IReferenceDataService referenceDataService, IMarketDataService marketDataService)
    {
        this.referenceDataService = referenceDataService;
        this.marketDataService = marketDataService;
    }

    public async Task AnalyzeStocksAsync()
    {
        var marketHolidays = await this.referenceDataService.GetMarketHolidaysAsync();

        _ = marketHolidays;
    }
}
