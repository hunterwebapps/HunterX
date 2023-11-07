using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Purchase.Interfaces;
using HunterX.Trader.Domain.Purchase.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

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

    private readonly SemaphoreSlim executionLock = new(1, 1);
    public async Task ExecuteDecisionAsync(ExecutionDecision executionDecision)
    {
        await this.executionLock.WaitAsync();

        try
        {
            var buyingPower = await this.brokerService.GetBuyingPowerAsync();

            var maxRiskPrice = buyingPower * 0.02m;

            var riskPerShare = executionDecision.Price - (executionDecision.Stop ?? 0);

            var shares = (int)Math.Floor(maxRiskPrice / riskPerShare);

            if (shares * executionDecision.Price > buyingPower)
            {
                shares = (int)Math.Floor(buyingPower / executionDecision.Price);
            }

            if (shares == 0)
            {
                var logMessage = "Not enough buying power to execute buy order for {symbol} at ${price} with {shares} shares based on a max risk price of {maxRiskPrice} and risk per share of {riskPerShare}";
                Logger.Warning(logMessage, executionDecision.Symbol, executionDecision.Price, shares, maxRiskPrice, riskPerShare);
                return;
            }
            else
            {
                var logMessage = "Executing Buy Order for {symbol} at ${price} with {shares} shares based on a max risk price of {maxRiskPrice} and risk per share of {riskPerShare}";
                Logger.Information(logMessage, executionDecision.Symbol, executionDecision.Price, shares, maxRiskPrice, riskPerShare);

                var order = await this.brokerService.ExecuteBuyOrderAsync(executionDecision, shares);

                await this.orderRepository.SaveOrderAsync(order);
            }

        }
        finally
        {
            this.executionLock.Release();
        }
    }

    private async void HandleOrderFilled(object? sender, OrderUpdated orderUpdated)
    {
        await this.orderRepository.UpdateOrderAsync(orderUpdated);
    }
}
