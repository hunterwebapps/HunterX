using HunterX.Trader.Domain.Purchase.ValueObjects;
using MassTransit;

namespace HunterX.Trader.Workers.Executor;

public class SellingMonitorConsumer : IConsumer<Order>
{
    public async Task Consume(ConsumeContext<Order> context)
    {
        
    }
}
