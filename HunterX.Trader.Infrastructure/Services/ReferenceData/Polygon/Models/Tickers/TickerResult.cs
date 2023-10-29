using System.Text.Json.Serialization;

namespace HunterX.Trader.Infrastructure.Services.ReferenceData.Polygon.Models.Tickers;

internal class TickerResult
{
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("cik")]
    public string CIK { get; set; } = default!;
    [JsonPropertyName("composite_figi")]
    public string CompositeFigi { get; set; } = default!;
    [JsonPropertyName("currency_name")]
    public string CurrencyName { get; set; } = default!;
    [JsonPropertyName("last_updated_utc")]
    public DateTime LastUpdatedUtc { get; set; }
    [JsonPropertyName("locale")]
    public string Locale { get; set; } = default!;
    [JsonPropertyName("market")]
    public string Market { get; set; } = default!;
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [JsonPropertyName("primary_exchange")]
    public string PrimaryExchange { get; set; } = default!;
    [JsonPropertyName("share_class_figi")]
    public string ShareClassFIGI { get; set; } = default!;
    [JsonPropertyName("ticker")]
    public string Ticker { get; set; } = default!;
    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;
}
