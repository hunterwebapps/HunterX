using HunterX.Trader.Domain.Common.Interfaces.Repositories;
using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.Purchases.Interfaces;
using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;

namespace HunterX.Trader.Application.Managers;

public class OrderManager
{
    private readonly IBrokerService brokerService;
    private readonly IOrderStreamingService orderStreamingService;
    private readonly IOrderRepository orderRepository;

    public OrderManager(IBrokerService brokerService, IOrderStreamingService orderStreamingService, IOrderRepository orderRepository)
    {
        this.brokerService = brokerService;
        this.orderStreamingService = orderStreamingService;
        this.orderRepository = orderRepository;

        this.orderStreamingService.OrderFilled += HandleOrderFilled;
    }

    public async Task SyncOrderDataAsync()
    {
        var openTrackedOrders = await this.orderRepository.GetOpenOrdersAsync();

        foreach (var openOrder in openTrackedOrders)
        {
            // TODO: Confirm if any properties have changed.
            var order = await this.brokerService.GetOrderByIdAsync(openOrder.OrderId);

            await this.orderRepository.UpdateOrderAsync(new OrderUpdated()
            {
                CancelledAt = order.CancelledAt,
                ExpiredAt = order.ExpiredAt,
                FailedAt = order.FailedAt,
                FilledAt = order.FilledAt,
                FilledPrice = order.FilledPrice,
                OrderId = order.OrderId,
                Symbol = order.Symbol,
                UpdatedAt = order.UpdatedAt,
            });
        }

        var openOrders = await this.brokerService.GetOpenOrdersAsync();

        var openUntrackedOrders = openOrders.Where(x => !openTrackedOrders.Any(y => y.OrderId == x.OrderId));

        foreach (var openUntrackedOrder in openUntrackedOrders)
        {
            await this.orderRepository.SaveOrderAsync(openUntrackedOrder);
        }
    }

    private async void HandleOrderFilled(object? sender, OrderUpdated orderUpdated)
    {
        await this.orderRepository.UpdateOrderAsync(orderUpdated);
    }
}
