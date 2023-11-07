using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.StrategySelection;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.Enum;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Application.Managers;

public class AnalysisManager
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IMarketDataService marketDataService;
    private readonly IBuySignalService buySignalService;
    private readonly IExecutionDecisionDetailsRepository executionDecisionDetailsRepository;

    public AnalysisManager(
        IDateTimeProvider dateTimeProvider,
        IMarketDataService marketDataService,
        IBuySignalService buySignalService,
        IExecutionDecisionDetailsRepository executionDecisionDetailsRepository)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.marketDataService = marketDataService;
        this.buySignalService = buySignalService;
        this.executionDecisionDetailsRepository = executionDecisionDetailsRepository;
    }

    public async Task SubmitBuySignalsAsync(StockBasics stockBasics)
    {
        var bars = await this.marketDataService.GetChartDataAsync(new ChartDataParams()
        {
            Symbol = stockBasics.Symbol,
            StartDate = this.dateTimeProvider.Now.Date,
            EndDate = this.dateTimeProvider.Now.DateTime,
            TimeFrame = "1min",
        });

        var strategySelection = new StrategySelectionRoot(stockBasics, bars, this.dateTimeProvider);

        var decisionDetails = strategySelection.GetExecutionDecisions();

        var buyExecutionDecisionDetails = decisionDetails.Where(details =>
        {
            if (details.ExecutionDecision.Action == TradeAction.Buy)
            {
                Logger.Information("Determined a buy signal for {symbol}.", details.ExecutionDecision.Symbol);
                return true;
            }

            if (details.ExecutionDecision.Action == TradeAction.Short)
            {
                Logger.Information("Determined a short signal for {symbol}.", details.ExecutionDecision.Symbol);
                return true;
            }

            Logger.Information("Determined not to act on {symbol}.", details.ExecutionDecision.Symbol);
            return false;
        });

        var buyExecutionDecisions = buyExecutionDecisionDetails.Select(details => details.ExecutionDecision);

        await this.executionDecisionDetailsRepository.SaveDecisionDetailsAsync(decisionDetails);

        await this.buySignalService.SubmitBuySignalsAsync(buyExecutionDecisions);
    }
}
