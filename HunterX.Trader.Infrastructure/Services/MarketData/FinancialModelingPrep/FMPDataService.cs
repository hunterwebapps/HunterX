using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Common.Serializers;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep.Models;
using HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep.Models.TechnicalIndicators;
using System.Text.Json;

namespace HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep;

public class FMPDataService
{
    private const string apiDomain = "https://financialmodelingprep.com";
    private readonly string apiKey;
    private readonly ApiThrottler apiThrottler;
    private readonly IHttpClientFactory httpClientFactory;

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new DateTimeConverterUsingDateTimeParse() },
    };

    public FMPDataService(AppSettings appSettings, IHttpClientFactory httpClientFactory)
    {
        this.apiKey = appSettings.FinancialModelingPrep.Key;
        this.apiThrottler = new ApiThrottler(appSettings.FinancialModelingPrep.RequestsPerSecond, TimeSpan.FromSeconds(1));
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyList<StockBasics>> GetStocksAsync()
    {
        Logger.Information("Fetching Potential Stocks from FinancialModelingPrep.");

        var stocks = new List<StockBasics>();

        var result = await GetRequestAsync<StockResponse[]>("/api/v3/stock-screener?isActivelyTrading=true&country=US&exchange=nyse,nasdaq,amex&volumeMoreThan=1000000&limit=5000");

        stocks.AddRange(result
            .Where(t => !string.IsNullOrEmpty(t.Symbol)
                && !string.IsNullOrEmpty(t.ExchangeShortName)
                && !string.IsNullOrEmpty(t.CompanyName)
                && t.Beta != null
                && t.Beta != 0
                && t.Price > 0)
            .Select(t => new StockBasics()
            {
                Symbol = t.Symbol!,
                Volume = t.Volume ?? 0,
                Sector = string.IsNullOrEmpty(t.Sector) ? null : t.Sector,
                Beta = t.Beta!.Value,
                CompanyName = t.CompanyName!,
                Exchange = t.ExchangeShortName!,
                Industry = t.Industry,
                MarketCap = t.MarketCap == 0 ? null : t.MarketCap,
                Price = t.Price!.Value,
                IsETF = t.IsETF ?? false,
            }));

        Logger.Information("Found {count} Stocks from FinancialModelingPrep.", stocks.Count);

        return stocks;
    }

    public async Task<IReadOnlyList<ChartData>> GetChartDataAsync(ChartDataParams parameters)
    {
        using var s1 = Logger.AddScope("Chart Data Params", parameters, destructureObject: true);

        Logger.Information("Fetching Chart Data from FinancialModelingPrep.");

        var result = await GetRequestAsync<SimpleMovingAverageResponse[]>($"/api/v3/historical-chart/{parameters.TimeFrame}/{parameters.Symbol}?from={parameters.StartDate:yyyy-MM-dd}&to={parameters.EndDate:yyyy-MM-dd}");

        var simpleMovingAverages = result
            .Select(t => new ChartData()
            {
                Symbol = parameters.Symbol,
                Date = t.Date,
                Open = t.Open,
                Close = t.Close,
                High = t.High,
                Low = t.Low,
                Volume = t.Volume,
            })
            .ToList();

        Logger.Information("Found {count} Chart Data entries.", simpleMovingAverages.Count);

        return simpleMovingAverages;
    }

    private async Task<T> GetRequestAsync<T>(string relativeUrl)
    {
        var client = this.httpClientFactory.CreateClient();

        var url = $"{apiDomain}{relativeUrl}&apikey={this.apiKey}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var cancellationToken = new CancellationToken(false);
        var response = await this.apiThrottler.ThrottledRequestAsync(() => client.SendAsync(request, cancellationToken), cancellationToken);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(json, jsonSerializerOptions) ?? throw new JsonException("Received null from json deserialization.");

        return result;
    }
}
