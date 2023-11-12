using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Interfaces.Repositories;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Application.Managers;

public class ExecutionDecisionManager
{
    private readonly IExecutionDecisionDetailsRepository executionDecisionDetailsRepository;

    public ExecutionDecisionManager(IExecutionDecisionDetailsRepository executionDecisionDetailsRepository)
    {
        this.executionDecisionDetailsRepository = executionDecisionDetailsRepository;
    }

    public async Task<PaginatedList<ExecutionDecisionBasics>> GetExecutionDecisionBasicsAsync(int pageNumber, int pageSize)
    {
        var paginatedBasics = await this.executionDecisionDetailsRepository.GetExecutionDecisionBasicsAsync(pageNumber, pageSize);

        return paginatedBasics;
    }

    public async Task<ExecutionDecisionDetails> GetExecutionDecisionDetailsAsync(Guid id)
    {
        var details = await this.executionDecisionDetailsRepository.GetExecutionDecisionDetails(id);

        return details;
    }
}
