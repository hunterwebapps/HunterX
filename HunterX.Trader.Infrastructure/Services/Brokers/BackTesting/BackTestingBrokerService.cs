using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.Purchase.Enums;
using HunterX.Trader.Domain.Purchase.Interfaces;
using HunterX.Trader.Domain.Purchase.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Infrastructure.Services.Brokers.BackTesting;

public class BackTestingBrokerService : IBrokerService
{
    private readonly IMarketDataService marketDataService;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly BackTestingOrderStreamingService backTestingStreamingService;

    public BackTestingBrokerService(IMarketDataService marketDataService, BackTestingOrderStreamingService backTestingStreamingService, IDateTimeProvider dateTimeProvider)
    {
        this.marketDataService = marketDataService;
        this.backTestingStreamingService = backTestingStreamingService;
        this.dateTimeProvider = dateTimeProvider;
    }

    public Task<decimal> GetBuyingPowerAsync()
    {
        return Task.FromResult(10_000m);
    }

    public Task<Order> ExecuteBuyOrderAsync(ExecutionDecision executionDecision, int quantity)
    {
        var order = new Order()
        {
            AssetClass = AssetClass.Stocks,
            CancelledAt = null,
            CreatedAt = this.dateTimeProvider.Now.DateTime,
            ExpiredAt = null,
            FailedAt = null,
            FilledAt = null,
            FilledPrice = null,
            Legs = new List<Order>()
            {
                new Order()
                {
                    AssetClass = AssetClass.Stocks,
                    CancelledAt = null,
                    CreatedAt = this.dateTimeProvider.Now.DateTime,
                    ExpiredAt = null,
                    FailedAt = null,
                    FilledAt = null,
                    FilledPrice = null,
                    Legs = new List<Order>(),
                    LimitPrice = null,
                    OrderedPrice = executionDecision.Price,
                    OrderId = Guid.NewGuid(),
                    OrderSide = OrderSide.Sell,
                    OrderType = OrderType.Stop,
                    Quantity = quantity,
                    StopLossPrice = executionDecision.Stop,
                    StopOrderPrice = executionDecision.Stop,
                    Symbol = executionDecision.Symbol,
                    TimeInForce = TimeInForce.GoodTilCancel,
                    TrailPercent = null,
                    TrailPrice = null,
                    UpdatedAt = null,
                },
            },
            LimitPrice = null,
            OrderedPrice = executionDecision.Price,
            OrderId = Guid.NewGuid(),
            OrderSide = OrderSide.Buy,
            OrderType = OrderType.Market,
            Quantity = quantity,
            StopLossPrice = executionDecision.Stop,
            StopOrderPrice = executionDecision.Stop,
            Symbol = executionDecision.Symbol,
            TimeInForce = TimeInForce.FillOrKill,
            TrailPercent = null,
            TrailPrice = null,
            UpdatedAt = null,
        };

        _ = Task.Run(async () =>
        {
            Logger.Information("Executing Task for Back Testing a Filled Order.");

            var orderPrices = await this.marketDataService.GetOrderPricesAsync(
                executionDecision.Symbol,
                this.dateTimeProvider.Now.AddHours(1).DateTime,
                this.dateTimeProvider.Now.DateTime);

            var averageSlippage = orderPrices.Sum(x => x.AskPrice - x.BidPrice) / orderPrices.Count * 0.6m;

            Logger.Information("Average Slippage: {AverageSlippage}", averageSlippage);

            this.backTestingStreamingService.SendFilledOrder(new OrderUpdated()
            {
                Position = quantity,
                FilledPrice = executionDecision.Price * quantity + averageSlippage,
                FilledAt = this.dateTimeProvider.Now.DateTime,
                Symbol = executionDecision.Symbol,
                CancelledAt = null,
                ExpiredAt = null,
                FailedAt = null,
                OrderId = order.OrderId,
                UpdatedAt = this.dateTimeProvider.Now.DateTime,
            });
        });

        return Task.FromResult(order);
    }

    public Task<Order> ExecuteSellOrderAsync(ExecutionDecision executionDecision, int quantity)
    {
        throw new NotImplementedException();
    }
}



