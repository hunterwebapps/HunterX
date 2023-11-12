using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Repositories;

public interface IExecutionDecisionDetailsRepository
{
    Task<ExecutionDecisionDetails> GetExecutionDecisionDetails(Guid id);
    Task<PaginatedList<ExecutionDecisionBasics>> GetExecutionDecisionBasicsAsync(int pageNumber, int pageSize);
    Task SaveDecisionDetailsAsync(ExecutionDecisionDetails decisionDetails);
}
