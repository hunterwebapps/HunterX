using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using MassTransit;

namespace HunterX.Trader.Infrastructure.Messaging;

public class StockFeedPublisher : ISymbolFeedService
{
    private readonly IBus bus;

    public StockFeedPublisher(IBus bus)
    {
        this.bus = bus;
    }

    public async Task SubmitSymbolsForProcessingAsync(IEnumerable<StockBasics> stocks)
    {
        await this.bus.PublishBatch(stocks);
    }
}
