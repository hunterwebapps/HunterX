using HunterX.Trader.Domain.Purchase.ValueObjects;

namespace HunterX.Trader.Application.Interfaces;

public interface IOrderRepository
{
    Task SaveOrderAsync(Order order);
    Task UpdateOrderAsync(OrderUpdated order);
}
