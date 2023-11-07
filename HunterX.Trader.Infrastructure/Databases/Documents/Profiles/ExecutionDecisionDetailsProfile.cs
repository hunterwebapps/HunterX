using AutoMapper;

namespace HunterX.Trader.Infrastructure.Databases.Documents.Profiles;

public class ExecutionDecisionDetailsProfile : Profile
{
    public ExecutionDecisionDetailsProfile()
    {
        CreateMap<ExecutionDecisionDetails, Domain.StrategySelection.Strategies.DecisionData.ExecutionDecisionDetails>()
            .ConvertUsing(x => EntityToDomain(x));
    }

    private static Domain.StrategySelection.Strategies.DecisionData.ExecutionDecisionDetails EntityToDomain(ExecutionDecisionDetails executionDecisionDetails)
    {
        return new Domain.StrategySelection.Strategies.DecisionData.ExecutionDecisionDetails(Guid.Parse(executionDecisionDetails._id))
        {
            Indicators = new Domain.StrategySelection.Strategies.DecisionData.ValueObjects.TechnicalIndicators()
            {
                MovingAverageConvergenceDivergence = executionDecisionDetails.Indicators.MovingAverageConvergenceDivergence,
                PivotPoints = executionDecisionDetails.Indicators.PivotPoints,
                Pivots = executionDecisionDetails.Indicators.Pivots,
                RollingPivots = executionDecisionDetails.Indicators.RollingPivots,
                AverageTrueRange = executionDecisionDetails.Indicators.AverageTrueRange,
                ShortEMA = executionDecisionDetails.Indicators.ShortEMA,
                RelativeStrengthIndex = executionDecisionDetails.Indicators.RelativeStrengthIndex,
                LongEMA = executionDecisionDetails.Indicators.LongEMA,
                VolumeWeightedAvgPrice = executionDecisionDetails.Indicators.VolumeWeightedAvgPrice,
            },
            ExecutionDecision = executionDecisionDetails.ExecutionDecision,
            StockBasics = executionDecisionDetails.StockBasics,
            Bars = executionDecisionDetails.Bars,
            StrategyName = executionDecisionDetails.StrategyName,
            CreatedAt = executionDecisionDetails.CreatedAt,
        };
    }
}
