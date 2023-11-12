using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Services;

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
