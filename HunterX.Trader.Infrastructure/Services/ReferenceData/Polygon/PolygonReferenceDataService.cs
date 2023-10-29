using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using HunterX.Trader.Infrastructure.Databases.Repositories;
using HunterX.Trader.Infrastructure.Services.ReferenceData.Polygon.Models.Holidays;
using HunterX.Trader.Infrastructure.Services.ReferenceData.Polygon.Models.Tickers;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HunterX.Trader.Infrastructure.Services.ReferenceData.Polygon;

public class PolygonReferenceDataService
{
    private const string apiDomain = "https://api.polygon.io";
    private readonly string apiKey;
    private readonly ApiThrottler apiThrottler;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ReferenceDataRepository referenceDataRepository;

    public PolygonReferenceDataService(AppSettings appSettings, IHttpClientFactory httpClientFactory, ReferenceDataRepository referenceDataRepository)
    {
        apiKey = appSettings.PolygonKey;
        apiThrottler = new ApiThrottler(5, TimeSpan.FromMinutes(1));
        this.httpClientFactory = httpClientFactory;
        this.referenceDataRepository = referenceDataRepository;
    }

    public async Task<IReadOnlyList<TickerSymbol>> GetSymbolsAsync()
    {
        var tickers = await this.referenceDataRepository.GetTickerSymbolsAsync();

        if (tickers.Count > 0 && tickers[0].Created > DateTime.UtcNow.Date)
        {
            Logger.Information("Using {Count} Ticker Symbols from Database.", tickers.Count);
            return tickers;
        }

        Logger.Information("Fetching Ticker Symbols from Polygon.");

        var updatedTickers = new List<TickerSymbol>();

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var url = $"{apiDomain}/v3/reference/tickers?market=stocks&active=true&sort=last_updated_utc&order=desc&limit=1000";

        while (url != null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var cancellationToken = new CancellationToken(false);
            var response = await apiThrottler.ThrottledRequestAsync(() => client.SendAsync(request, cancellationToken), cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PolygonTickersResponse>(json);

            if (result == null || result.Status != "OK")
            {
                var exception = new Exception("Failed to deserialize tickers response.");
                Logger.Error(exception, "Failed to deserialize tickers response {ResponseContent} for {Url}", json, url);
                throw exception;
            }

            updatedTickers.AddRange(result.Results.Select(t => new TickerSymbol(t.Ticker, t.Name, t.PrimaryExchange, MakeMarketType(t.Market), DateTime.UtcNow)));

            url = result.NextUrl;
        }

        Logger.Information("Found {Count} Ticker Symbols from Polygon. Saving to Database.", updatedTickers.Count);

        await this.referenceDataRepository.InsertTickerSymbolsAsync(updatedTickers);

        return updatedTickers;
    }

    public async Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync()
    {
        var holidays = await this.referenceDataRepository.GetMarketHolidaysAsync();

        if (holidays.Count > 0 && holidays[0].Created > DateTime.UtcNow.Date.AddMonths(-1))
        {
            Logger.Information("Using {Count} Market Holidays from Database.", holidays.Count);
            return holidays;
        }

        Logger.Information("Fetching Market Holidays from Polygon.");

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var url = $"{apiDomain}/v1/marketstatus/upcoming";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var cancellationToken = new CancellationToken(false);
        var response = await apiThrottler.ThrottledRequestAsync(() => client.SendAsync(request, cancellationToken), cancellationToken);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PolygonUpcomingMarketStatusResponse[]>(json);

        if (result == null)
        {
            var exception = new Exception("Failed to deserialize upcoming holidays response.");
            Logger.Error(exception, "Failed to deserialize upcoming holidays response {ResponseContent}", json);
            throw exception;
        }

        var updatedHolidays = result
            .Select(r =>
            {
                var openTime = r.Open == null ? (TimeOnly?)null : TimeOnly.FromDateTime(r.Open.Value);
                var closeTime = r.Close == null ? (TimeOnly?)null : TimeOnly.FromDateTime(r.Close.Value);
                return new MarketHoliday(r.Date, openTime, closeTime, r.Exchange, r.Name, DateTime.UtcNow);
            })
            .ToList();

        Logger.Information("Found {Count} Market Holidays from Polygon. Saving to Database.", updatedHolidays.Count);

        await referenceDataRepository.InsertMarketHolidaysAsync(updatedHolidays);

        return updatedHolidays;
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
