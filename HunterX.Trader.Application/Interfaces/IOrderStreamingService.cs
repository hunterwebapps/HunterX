using HunterX.Trader.Domain.Purchase.ValueObjects;

namespace HunterX.Trader.Application.Interfaces;

public interface IOrderStreamingService : IDisposable
{
    event EventHandler? Connected;
    event EventHandler? Disconnected;

    event EventHandler<OrderUpdated>? OrderFilled;
    event EventHandler<OrderUpdated>? OrderPartiallyFilled;
    event EventHandler<string>? OrderCancelled;
    event EventHandler<string>? OrderRejected;

    Task ConnectAsync();
    Task DisconnectAsync();
}
