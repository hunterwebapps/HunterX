using System.Text.Json.Serialization;

namespace HunterX.Trader.Infrastructure.Services.ReferenceData.Polygon.Models.Tickers;

internal class PolygonTickersResponse
{
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = default!;
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("next_url")]
    public string NextUrl { get; set; } = default!;
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("results")]
    public TickerResult[] Results { get; set; } = default!;
}