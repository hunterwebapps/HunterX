using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.Purchase.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using HunterX.Trader.Infrastructure.Services.MarketData.Alpaca;
using HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep;
using HunterX.Trader.Infrastructure.Services.MarketData.Polygon;

namespace HunterX.Trader.Infrastructure.Services.MarketData;

public class MarketDataService : IMarketDataService
{
    private readonly FMPDataService financialModelingPrepService;
    private readonly PolygonDataService polygonService;
    private readonly AlpacaDataService alpacaService;

    public MarketDataService(FMPDataService financialModelingPrepService, PolygonDataService polygonService, AlpacaDataService alpacaService)
    {
        this.financialModelingPrepService = financialModelingPrepService;
        this.polygonService = polygonService;
        this.alpacaService = alpacaService;
    }

    public async Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync()
    {
        return await this.polygonService.GetMarketHolidaysAsync();
    }

    public async Task<IReadOnlyList<StockBasics>> GetStocksAsync()
    {
        return await this.financialModelingPrepService.GetStocksAsync();
    }

    public async Task<IReadOnlyList<ChartData>> GetChartDataAsync(ChartDataParams parameters)
    {
        return await this.financialModelingPrepService.GetChartDataAsync(parameters);
    }

    public async Task<IReadOnlyList<OrderPrice>> GetOrderPricesAsync(string symbol, DateTime from, DateTime to)
    {
        return await this.alpacaService.GetOrderPriceAsync(symbol, from, to);
    }
}
