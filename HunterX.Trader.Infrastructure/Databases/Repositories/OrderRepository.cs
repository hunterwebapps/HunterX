using AutoMapper;
using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.Purchase.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HunterX.Trader.Infrastructure.Databases.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly TradingSqlDbContext tradingDbContext;
    private readonly IMapper mapper;

    public OrderRepository(TradingSqlDbContext tradingDbContext, IMapper mapper)
    {
        this.tradingDbContext = tradingDbContext;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyList<Order>> GetOrdersAsync()
    {
        var entities = await this.tradingDbContext.Orders.ToListAsync();

        var orders = this.mapper.Map<IReadOnlyList<Order>>(entities);

        return orders;
    }

    public async Task<Order> GetOrderAsync(Guid orderId)
    {
        var entity = await this.tradingDbContext.Orders.FindAsync(orderId);

        var order = this.mapper.Map<Order>(entity);

        return order;
    }

    public async Task SaveOrderAsync(Order order)
    {
        var entity = this.mapper.Map<Entities.Order>(order);

        await this.tradingDbContext.Orders.AddAsync(entity);

        await this.tradingDbContext.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(OrderUpdated order)
    {
        var entity = await this.tradingDbContext.Orders.FindAsync(order.OrderId);

        if (entity == null)
        {
            throw new Exception($"Order with id {order.OrderId} not found");
        }

        entity.FilledAt = order.FilledAt;
        entity.FilledPrice = order.FilledPrice;
        entity.UpdatedAt = order.UpdatedAt;
        entity.ExpiredAt = order.ExpiredAt;
        entity.CancelledAt = order.CancelledAt;
        entity.FailedAt = order.FailedAt;
        
        this.tradingDbContext.Orders.Update(entity);

        await this.tradingDbContext.SaveChangesAsync();
    }
}
