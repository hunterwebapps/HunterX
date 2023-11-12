using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.StrategySelections.Universe.Enums;

namespace HunterX.Trader.Domain.Trading.StrategySelections.Universe.ValueObjects;

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
        Sector = sector;
        Industry = industry;
        MarketCapMin = marketCapMin;
        MarketCapMax = marketCapMax;
        PriceMin = priceMin;
        PriceMax = priceMax;
        BetaMin = betaMin;
        BetaMax = betaMax;
        VolumeMin = volumeMin;
        VolumeMax = volumeMax;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Sector;
        yield return Industry;
        yield return MarketCapMin;
        yield return MarketCapMax;
        yield return PriceMin;
        yield return PriceMax;
        yield return VolumeMin;
        yield return VolumeMax;
    }
}
