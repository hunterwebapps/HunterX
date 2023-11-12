using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.StrategySelections;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Domain.Trading;

public class TradingRoot : AggregateRoot
{
    private readonly Dictionary<string, IStrategy> activeStrategies = new();

    public bool IsTrading(string symbol) => this.activeStrategies.ContainsKey(symbol);

    public void StartTrading(Asset stock, IReadOnlyList<Bar> bars)
    {
        var strategySelection = new StrategySelection(stock, bars);

        var strategy = strategySelection.DetermineBestStrategy();

        if (strategy == null)
        {
            Logger.Information("No Viable Strategy Found for {symbol}", stock.Symbol);
            return;
        }

        this.activeStrategies.Add(stock.Symbol, strategy);
    }

    public void StopTrading(string symbol)
    {
        this.activeStrategies.Remove(symbol);
    }

    public ExecutionDecisionDetails EvaluateBarDecision(Bar bar)
    {
        var strategy = this.activeStrategies[bar.Symbol];

        var decision = strategy.EvaluateLatestBar(bar);

        return new ExecutionDecisionDetails(Guid.NewGuid())
        {
            Bars = strategy.Bars,
            ExecutionDecision = decision,
            Stock = strategy.Stock,
            StrategyName = strategy.Name,
        };
    }
}
