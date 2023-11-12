using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Common.Serializers;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;
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

    public async Task<IReadOnlyList<Asset>> GetStocksAsync()
    {
        Logger.Information("Fetching Potential Stocks from FinancialModelingPrep.");

        var stocks = new List<Asset>();

        var result = await GetRequestAsync<StockResponse[]>("/api/v3/stock-screener", new()
        {
            ["isActivelyTrading"] = "true",
            ["exchange"] = "nyse,nasdaq,amex",
            ["volumeMoreThan"] = "1000000",
            ["betaMoreThan"] = "3",
            ["priceMoreThan"] = "1",
            ["limit"] = "5000",
        });

        stocks.AddRange(result
            .Where(t => !string.IsNullOrEmpty(t.Symbol)
                && !string.IsNullOrEmpty(t.ExchangeShortName)
                && !string.IsNullOrEmpty(t.CompanyName)
                && t.Beta != null
                && t.Beta != 0
                && t.Price > 0)
            .Select(t => new Asset()
            {
                Symbol = t.Symbol!,
                CompanyName = t.CompanyName!,
            }));

        Logger.Information("Found {count} Stocks from FinancialModelingPrep.", stocks.Count);

        return stocks;
    }

    public async Task<IReadOnlyList<Asset>> GetStocksAsync(IEnumerable<string> symbols)
    {
        Logger.Information("Fetching Specific Stocks from FinancialModelingPrep.");

        var stocks = new List<Asset>();

        var tasks = new List<Task<StockResponse>>();
        foreach (var symbol in symbols)
        {
            var task = GetRequestAsync<StockResponse>($"/api/v3/profile/{symbol}", new());

            tasks.Add(task);
        }

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            stocks.Add(new Asset()
            {
                Symbol = result.Symbol!,
                CompanyName = result.CompanyName!,
            });
        }

        Logger.Information("Found {count} Specific Stocks from FinancialModelingPrep.", stocks.Count);

        return stocks;
    }

    public async Task<IReadOnlyList<Bar>> GetChartDataAsync(ChartDataParams parameters)
    {
        using var s1 = Logger.AddScope("Chart Data Params", parameters, destructureObject: true);

        Logger.Information("Fetching Chart Data from FinancialModelingPrep.");

        var result = await GetRequestAsync<SimpleMovingAverageResponse[]>($"/api/v3/historical-chart/{parameters.TimeFrame}/{parameters.Symbol}", new()
        {
            ["from"] = parameters.StartDate.ToString("yyyy-MM-dd"),
            ["to"] = parameters.EndDate.ToString("yyyy-MM-dd"),
        });

        var simpleMovingAverages = result
            .Select(t => new Bar()
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

    private async Task<T> GetRequestAsync<T>(string relativePath, Dictionary<string, string> queryParams)
    {
        var client = this.httpClientFactory.CreateClient();

        var queryString = string.Join("&", queryParams.Select(t => $"{t.Key}={t.Value}"));

        var url = $"{apiDomain}{relativePath}?{queryString}&apikey={this.apiKey}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var cancellationToken = new CancellationToken(false);
        var response = await this.apiThrottler.ThrottledRequestAsync(() => client.SendAsync(request, cancellationToken), cancellationToken);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(json, jsonSerializerOptions) ?? throw new JsonException("Received null from json deserialization.");

        return result;
    }
}
