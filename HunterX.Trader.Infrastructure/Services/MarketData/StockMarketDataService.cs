using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;
using HunterX.Trader.Infrastructure.Services.MarketData.Alpaca;
using HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep;
using HunterX.Trader.Infrastructure.Services.MarketData.Polygon;

namespace HunterX.Trader.Infrastructure.Services.MarketData;

public class StockMarketDataService : IMarketDataService
{
    private readonly FMPDataService financialModelingPrepService;
    private readonly PolygonDataService polygonService;
    private readonly AlpacaDataService alpacaService;

    public StockMarketDataService(FMPDataService financialModelingPrepService, PolygonDataService polygonService, AlpacaDataService alpacaService)
    {
        this.financialModelingPrepService = financialModelingPrepService;
        this.polygonService = polygonService;
        this.alpacaService = alpacaService;
    }

    public async Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync()
    {
        return await this.polygonService.GetMarketHolidaysAsync();
    }

    public async Task<IReadOnlyList<Asset>> GetAssetsAsync()
    {
        return await this.financialModelingPrepService.GetStocksAsync();
    }

    public async Task<IReadOnlyList<Asset>> GetAssetsAsync(IEnumerable<string> symbols)
    {
        return await this.financialModelingPrepService.GetStocksAsync(symbols);
    }

    public async Task<IReadOnlyList<Bar>> GetChartDataAsync(ChartDataParams parameters)
    {
        return await this.financialModelingPrepService.GetChartDataAsync(parameters);
    }

    public async Task<IReadOnlyList<OrderPrice>> GetOrderPricesAsync(string symbol, DateTime from, DateTime to)
    {
        return await this.alpacaService.GetOrderPriceAsync(symbol, from, to);
    }
}
