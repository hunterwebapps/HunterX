using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using MassTransit;

namespace HunterX.Trader.Infrastructure.Messaging;

public class BuySignalPublisher : IBuySignalService
{
    private readonly IBus bus;

    public BuySignalPublisher(IBus bus)
    {
        this.bus = bus;
    }

    public async Task SubmitBuySignalsAsync(IEnumerable<ExecutionDecision> buySignals)
    {
        await this.bus.PublishBatch(buySignals);
    }
}

