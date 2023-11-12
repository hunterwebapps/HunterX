using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using MassTransit;

namespace HunterX.Trader.Infrastructure.Messaging;

public class StockFeedPublisher : ISymbolFeedService
{
    private readonly IBus bus;

    public StockFeedPublisher(IBus bus)
    {
        this.bus = bus;
    }

    public async Task SubmitSymbolsForProcessingAsync(IEnumerable<Asset> stocks)
    {
        await this.bus.PublishBatch(stocks);
    }
}
