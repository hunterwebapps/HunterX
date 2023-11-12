using AutoMapper;

namespace HunterX.Trader.Infrastructure.Databases.Documents.Profiles;

public class ExecutionDecisionDetailsProfile : Profile
{
    public ExecutionDecisionDetailsProfile()
    {
        CreateMap<ExecutionDecisionDetails, Domain.Trading.StrategySelections.Strategies.DecisionData.ExecutionDecisionDetails>()
            .ConvertUsing(x => EntityToDomain(x));
    }

    private static Domain.Trading.StrategySelections.Strategies.DecisionData.ExecutionDecisionDetails EntityToDomain(ExecutionDecisionDetails executionDecisionDetails)
    {
        return new Domain.Trading.StrategySelections.Strategies.DecisionData.ExecutionDecisionDetails(Guid.Parse(executionDecisionDetails._id))
        {
            ExecutionDecision = executionDecisionDetails.ExecutionDecision,
            Stock = executionDecisionDetails.StockBasics,
            Bars = executionDecisionDetails.Bars,
            StrategyName = executionDecisionDetails.StrategyName,
        };
    }
}
