using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using MassTransit;

namespace HunterX.Trader.Workers.Executor;

public class ExecutionDecisionConsumer : IConsumer<ExecutionDecision>
{
    private readonly OrderManager purchaseManager;

    public ExecutionDecisionConsumer(OrderManager purchaseManager)
    {
        this.purchaseManager = purchaseManager;
    }

    public async Task Consume(ConsumeContext<ExecutionDecision> context)
    {
        Logger.Information("Received {action} signal for {symbol}", context.Message.Action, context.Message.Symbol);

        await this.purchaseManager.ExecuteDecisionAsync(context.Message);
    }
}
