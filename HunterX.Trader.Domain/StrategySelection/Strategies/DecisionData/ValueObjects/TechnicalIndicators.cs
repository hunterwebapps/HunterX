using Skender.Stock.Indicators;

namespace HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

public record TechnicalIndicators
{
    public IReadOnlyList<PivotsResult> Pivots { get; set; } = new List<PivotsResult>();
    public IReadOnlyList<PivotPointsResult> PivotPoints { get; set; } = new List<PivotPointsResult>();
    public IReadOnlyList<RollingPivotsResult> RollingPivots { get; set; } = new List<RollingPivotsResult>();
    public IReadOnlyList<EmaResult> ShortEMA { get; init; } = new List<EmaResult>();
    public IReadOnlyList<EmaResult> LongEMA { get; init; } = new List<EmaResult>();
    public IReadOnlyList<MacdResult> MovingAverageConvergenceDivergence { get; set; } = new List<MacdResult>();
    public IReadOnlyList<VwmaResult> VolumeWeightedAvgPrice { get; init; } = new List<VwmaResult>();
    public IReadOnlyList<RsiResult> RelativeStrengthIndex { get; init; } = new List<RsiResult>();
    public IReadOnlyList<AtrResult> AverageTrueRange { get; init; } = new List<AtrResult>();
}
