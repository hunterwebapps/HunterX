using HunterX.Trader.Application.Services.Interfaces;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using HunterX.Trader.Infrastructure.Services.ReferenceData.FinancialModelingPrep.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HunterX.Trader.Infrastructure.Services.ReferenceData.FinancialModelingPrep;

public class FMPReferenceDataService
{
    private const string apiDomain = "https://financialmodelingprep.com";
    private readonly string apiKey;
    private readonly ApiThrottler apiThrottler;
    private readonly IHttpClientFactory httpClientFactory;

    public FMPReferenceDataService(AppSettings appSettings, IHttpClientFactory httpClientFactory)
    {
        apiKey = appSettings.PolygonKey;
        apiThrottler = new ApiThrottler(750, TimeSpan.FromMinutes(1));
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyList<StockBasics>> GetUniverseStockBasicsAsync(UniverseCriteria criteria)
    {
        Logger.Information("Fetching Ticker Symbols from Polygon.");

        var updatedTickers = new List<TickerSymbol>();

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var url = $"{apiDomain}/api/v3/stock/list";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var cancellationToken = new CancellationToken(false);
        var response = await apiThrottler.ThrottledRequestAsync(() => client.SendAsync(request, cancellationToken), cancellationToken);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StockResponse[]>(json) ?? throw new JsonException("Received null from json deserialization.");

        updatedTickers.AddRange(result.Select(t => new TickerSymbol(t.Symbol, t.CompanyName, t.ExchangeShortName, MakeMarketType("stocks"), DateTime.UtcNow)));

        Logger.Information("Found {Count} Ticker Symbols from Polygon. Saving to Database.", updatedTickers.Count);

        return null;
    }

    private static MarketType MakeMarketType(string market)
    {
        return market switch
        {
            "stocks" => MarketType.Stocks,
            "crypto" => MarketType.Crypto,
            _ => throw new Exception($"Unknown market type string {market}.")
        };
    }
}
