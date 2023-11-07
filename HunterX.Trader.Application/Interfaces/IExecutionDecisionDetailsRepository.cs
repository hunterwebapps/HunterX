using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Application.Interfaces;

public interface IExecutionDecisionDetailsRepository
{
    Task<ExecutionDecisionDetails> GetExecutionDecisionDetails(Guid id);
    Task<PaginatedList<ExecutionDecisionBasics>> GetExecutionDecisionBasicsAsync(int pageNumber, int pageSize);
    Task SaveDecisionDetailsAsync(IReadOnlyList<ExecutionDecisionDetails> decisionDetails);
}
