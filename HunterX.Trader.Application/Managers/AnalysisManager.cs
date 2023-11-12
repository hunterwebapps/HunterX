using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.Common.Interfaces.Repositories;
using HunterX.Trader.Domain.Common.Interfaces.Services;

namespace HunterX.Trader.Application.Managers;

public class AnalysisManager
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IMarketDataService marketDataService;
    private readonly IExecutionDecisionDetailsRepository executionDecisionDetailsRepository;

    public AnalysisManager(
        IDateTimeProvider dateTimeProvider,
        IMarketDataService marketDataService,
        IExecutionDecisionDetailsRepository executionDecisionDetailsRepository)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.marketDataService = marketDataService;
        this.executionDecisionDetailsRepository = executionDecisionDetailsRepository;
    }
}
