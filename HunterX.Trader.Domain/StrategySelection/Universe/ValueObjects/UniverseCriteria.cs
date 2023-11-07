using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Universe.Enums;

namespace HunterX.Trader.Domain.StrategySelection.Universe.ValueObjects;

public class UniverseCriteria : ValueObject
{
    public Sector? Sector { get; }
    public string? Industry { get; }
    public long? MarketCapMin { get; }
    public long? MarketCapMax { get; }
    public long? PriceMin { get; }
    public long? PriceMax { get; }
    public decimal? BetaMin { get; set; }
    public decimal? BetaMax { get; set; }
    public int MyProperty { get; set; }
    public int? VolumeMin { get; }
    public int? VolumeMax { get; }

    public UniverseCriteria(Sector? sector, string? industry, long? marketCapMin, long? marketCapMax, long? priceMin, long? priceMax, decimal? betaMin, decimal? betaMax, int? volumeMin, int? volumeMax)
    {
        this.Sector = sector;
        this.Industry = industry;
        this.MarketCapMin = marketCapMin;
        this.MarketCapMax = marketCapMax;
        this.PriceMin = priceMin;
        this.PriceMax = priceMax;
        this.BetaMin = betaMin;
        this.BetaMax = betaMax;
        this.VolumeMin = volumeMin;
        this.VolumeMax = volumeMax;
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
