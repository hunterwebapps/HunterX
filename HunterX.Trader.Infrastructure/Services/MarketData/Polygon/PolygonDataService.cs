using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Purchase.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using HunterX.Trader.Infrastructure.Services.MarketData.Polygon.Models.Holidays;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HunterX.Trader.Infrastructure.Services.MarketData.Polygon;

public class PolygonDataService
{
    private const string apiDomain = "https://api.polygon.io";
    private readonly string apiKey;
    private readonly ApiThrottler apiThrottler;
    private readonly IHttpClientFactory httpClientFactory;

    public PolygonDataService(AppSettings appSettings, IHttpClientFactory httpClientFactory)
    {
        this.apiKey = appSettings.PolygonKey;
        this.apiThrottler = new ApiThrottler(5, TimeSpan.FromMinutes(1));
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyList<MarketHoliday>> GetMarketHolidaysAsync()
    {
        Logger.Information("Fetching Market Holidays from Polygon.");

        var client = this.httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.apiKey);

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

        Logger.Information("Found {Count} Market Holidays from Polygon.", updatedHolidays.Count);

        return updatedHolidays;
    }
}