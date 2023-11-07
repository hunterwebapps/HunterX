using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.StrategySelection.Strategies;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Domain.StrategySelection;

public class StrategySelectionRoot : AggregateRoot
{
    private readonly StockBasics stockBasics;
    private readonly IReadOnlyList<ChartData> bars;
    private readonly IDateTimeProvider dateTimeProvider;

    public StrategySelectionRoot(StockBasics stockBasics, IReadOnlyList<ChartData> bars, IDateTimeProvider dateTimeProvider) : base(null)
    {
        this.stockBasics = stockBasics;
        this.bars = bars;
        this.dateTimeProvider = dateTimeProvider;
    }

    public IReadOnlyList<ExecutionDecisionDetails> GetExecutionDecisions()
    {
        var decisions = new List<ExecutionDecisionDetails>();

        var trendStrategy = new TrendFollowing(this.stockBasics, this.bars, this.dateTimeProvider);

        var executionDecision = trendStrategy.DetermineBuyDecision();

        decisions.Add(executionDecision);

        return decisions;
    }
}
