using System.Text.Json.Serialization;

namespace HunterX.Trader.Infrastructure.Services.MarketData.Polygon.Models.Holidays;

internal class PolygonUpcomingMarketStatusResponse
{
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }
    [JsonPropertyName("open")]
    public DateTime? Open { get; set; }
    [JsonPropertyName("close")]
    public DateTime? Close { get; set; }
    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = default!;
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
}