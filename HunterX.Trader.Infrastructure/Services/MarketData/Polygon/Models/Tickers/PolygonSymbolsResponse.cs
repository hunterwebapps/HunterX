using System.Text.Json.Serialization;

namespace HunterX.Trader.Infrastructure.Services.MarketData.Polygon.Models.Symbols;

internal class PolygonSymbolsResponse
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
    public SymbolResult[] Results { get; set; } = default!;
}