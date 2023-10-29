using HunterX.Trader.Domain.Common;

namespace HunterX.Trader.Domain.StrategySelection.ValueObjects;

public class UniverseCriteria : ValueObject
{
    public string? Sector { get; }
    public string? Industry { get; }
    public string? MarketCapMin { get; }
    public string? MarketCapMax { get; }
    public string? PriceMin { get; }
    public string? PriceMax { get; }
    public string? VolumeMin { get; }
    public string? VolumeMax { get; }

    public UniverseCriteria(string? sector, string? industry, string? marketCapMin, string? marketCapMax, string? priceMin, string? priceMax, string? volumeMin, string? volumeMax)
    {
        this.Sector = sector;
        this.Industry = industry;
        this.MarketCapMin = marketCapMin;
        this.MarketCapMax = marketCapMax;
        this.PriceMin = priceMin;
        this.PriceMax = priceMax;
        this.VolumeMin = volumeMin;
        this.VolumeMax = volumeMax;
    }

    public override string ToString()
    {
        return $"{this.Sector}{this.Industry}{this.MarketCapMin}{this.MarketCapMax}{this.PriceMin}{this.PriceMax}{this.VolumeMin}{this.VolumeMax}";
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return this.Sector;
        yield return this.Industry;
        yield return this.MarketCapMin;
        yield return this.MarketCapMax;
        yield return this.PriceMin;
        yield return this.PriceMax;
        yield return this.VolumeMin;
        yield return this.VolumeMax;
    }
}
