using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;

namespace HunterX.Trader.Domain.Common.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetOpenOrdersAsync();
    Task<IList<Order>> GetOrdersBySymbolAsync(string symbol);
    Task SaveOrderAsync(Order order);
    Task UpdateOrderAsync(OrderUpdated order);
}
