using HunterX.Trader.Application.Managers;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using MassTransit;

namespace HunterX.Trader.Workers.Analysis;

public class SymbolFeedConsumer : IConsumer<StockBasics>
{
    private readonly AnalysisManager analysisManager;

    public SymbolFeedConsumer(AnalysisManager analysisManager)
    {
        this.analysisManager = analysisManager;
    }

    public async Task Consume(ConsumeContext<StockBasics> context)
    {
        await this.analysisManager.SubmitBuySignalsAsync(context.Message);
    }
}
