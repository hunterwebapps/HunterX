using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
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

