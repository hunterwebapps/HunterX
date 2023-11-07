using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.Purchase.ValueObjects;

namespace HunterX.Trader.Infrastructure.Services.Brokers.BackTesting;

public class BackTestingOrderStreamingService : IOrderStreamingService
{
    public event EventHandler? Connected;
    public event EventHandler? Disconnected;
    public event EventHandler<OrderUpdated>? OrderFilled;
    public event EventHandler<OrderUpdated>? OrderPartiallyFilled;
    public event EventHandler<string>? OrderCancelled;
    public event EventHandler<string>? OrderRejected;

    public Task ConnectAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisconnectAsync()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void SendFilledOrder(OrderUpdated orderUpdated)
    {
        this.OrderFilled?.Invoke(this, orderUpdated);
    }
}
