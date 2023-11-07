using Alpaca.Markets;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Purchase.Interfaces;
using HunterX.Trader.Domain.Purchase.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.Enum;

namespace HunterX.Trader.Infrastructure.Services.Brokers.Alpaca;

public class AlpacaBrokerService : IBrokerService
{
    private readonly IAlpacaTradingClient tradingClient;

    public AlpacaBrokerService(AppSettings appSettings)
    {
        var secretKey = new SecretKey(appSettings.Alpaca.Key, appSettings.Alpaca.Secret);

        var environment = appSettings.Live
            ? Environments.Live
            : Environments.Paper;

        this.tradingClient = environment.GetAlpacaTradingClient(secretKey);

        Logger.Information("Alpaca Broker Service Initialized with Target {Url}", environment.AlpacaTradingApi);
    }

    public async Task<decimal> GetBuyingPowerAsync()
    {
        var account = await this.tradingClient.GetAccountAsync();

        return account.BuyingPower ?? 0;
    }

    public async Task<Order> ExecuteBuyOrderAsync(ExecutionDecision executionDecision, int quantity)
    {
        if (executionDecision.Action != TradeAction.Buy)
        {
            throw new ArgumentException("Trade Execution must be a Buy.");
        }

        var orderId = Guid.NewGuid();

        using var s1 = Logger.AddScope("Execution Decision", executionDecision, destructureObject: true);
        using var s2 = Logger.AddScope("Order Id", orderId);

        var orderRequest = OrderSide.Buy
            .StopLimit(executionDecision.Symbol, OrderQuantity.FromInt64(quantity), executionDecision.Stop, executionDecision.Limit)
            .StopLoss(executionDecision.StopLoss)
            .WithDuration(TimeInForce.Ioc)
            .WithClientOrderId(orderId.ToString());

        Logger.Information("Submitting Buy Order to Alpaca API {@OrderRequest}", orderRequest);

        var submittedOrder = await this.tradingClient.PostOrderAsync(orderRequest);

        Logger.Information("Successfully Executed Buy Order. {@order}", submittedOrder);

        var orderEntity = MakeOrder(submittedOrder, executionDecision);

        return orderEntity;
    }

    public async Task UpdateStopLossAsync(Guid orderId, decimal stopLoss)
    {
        var changeOrderRequest = new ChangeOrderRequest(orderId)
        {
            StopPrice = stopLoss,
        };

        var order = await this.tradingClient.PatchOrderAsync(changeOrderRequest);
    }

    public async Task<Order> ExecuteSellOrderAsync(ExecutionDecision executionDecision, int quantity)
    {
        if (executionDecision.Action != TradeAction.Sell)
        {
            throw new ArgumentException("Trade Execution must be a Sell.");
        }

        var orderId = Guid.NewGuid().ToString();

        using var s1 = Logger.AddScope("Execution Decision", executionDecision, destructureObject: true);
        using var s2 = Logger.AddScope("Order Id", orderId);

        Logger.Information("Submitting Sell Order to Alpaca API.");

        var orderRequest = new NewOrderRequest(
            symbol: executionDecision.Symbol,
            quantity: OrderQuantity.FromInt64(quantity),
            side: OrderSide.Sell,
            type: OrderType.Market,
            duration: TimeInForce.Fok)
        {
            ClientOrderId = orderId,
        };

        var submitttedOrder = await this.tradingClient.PostOrderAsync(orderRequest);

        Logger.Information("Successfully Executed Sell Order. {@order}", submitttedOrder);

        var orderEntity = MakeOrder(submitttedOrder, executionDecision);

        return orderEntity;
    }

    private static Order MakeOrder(IOrder order, ExecutionDecision executionDecision)
    {
        return new Order()
        {
            OrderId = order.OrderId,
            Symbol = order.Symbol,
            TimeInForce = MakeTimeInForce(order.TimeInForce),
            AssetClass = MakeAssetClass(order.AssetClass),
            OrderedPrice = executionDecision.Price,
            FilledPrice = order.AverageFillPrice,
            CreatedAt = order.CreatedAtUtc!.Value,
            LimitPrice = order.LimitPrice,
            OrderSide = MakeOrderSide(order.OrderSide),
            OrderType = MakeOrderType(order.OrderType),
            Quantity = (int)order.IntegerQuantity,
            StopOrderPrice = order.StopPrice,
            StopLossPrice = executionDecision.StopLoss,
            TrailPercent = order.TrailOffsetInPercent,
            TrailPrice = order.TrailOffsetInDollars,
            CancelledAt = order.CancelledAtUtc,
            ExpiredAt = order.ExpiredAtUtc,
            FailedAt = order.FailedAtUtc,
            FilledAt = order.FilledAtUtc,
            UpdatedAt = order.UpdatedAtUtc,
            Legs = order.Legs
                .Select((leg) => MakeOrder(leg, executionDecision))
                .ToList(),
        };
    }

    private static Domain.Purchase.Enums.AssetClass MakeAssetClass(AssetClass assetClass)
    {
        return assetClass switch
        {
            AssetClass.UsEquity => Domain.Purchase.Enums.AssetClass.Stocks,
            AssetClass.Crypto => Domain.Purchase.Enums.AssetClass.Crypto,
            _ => throw new ArgumentOutOfRangeException(nameof(assetClass), assetClass, null)
        };
    }

    private static Domain.Purchase.Enums.OrderSide MakeOrderSide(OrderSide orderSide)
    {
        return orderSide switch
        {
            OrderSide.Buy => Domain.Purchase.Enums.OrderSide.Buy,
            OrderSide.Sell => Domain.Purchase.Enums.OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null)
        };
    }

    private static Domain.Purchase.Enums.OrderType MakeOrderType(OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Market => Domain.Purchase.Enums.OrderType.Market,
            OrderType.Stop => Domain.Purchase.Enums.OrderType.Stop,
            OrderType.Limit => Domain.Purchase.Enums.OrderType.Limit,
            OrderType.StopLimit => Domain.Purchase.Enums.OrderType.StopLimit,
            OrderType.TrailingStop => Domain.Purchase.Enums.OrderType.TrailingStop,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }

    private static Domain.Purchase.Enums.TimeInForce MakeTimeInForce(TimeInForce timeInForce)
    {
        return timeInForce switch
        {
            TimeInForce.Day => Domain.Purchase.Enums.TimeInForce.Day,
            TimeInForce.Gtc => Domain.Purchase.Enums.TimeInForce.GoodTilCancel,
            TimeInForce.Opg => Domain.Purchase.Enums.TimeInForce.MarketOpen,
            TimeInForce.Fok => Domain.Purchase.Enums.TimeInForce.FillOrKill,
            TimeInForce.Cls => Domain.Purchase.Enums.TimeInForce.MarketClose,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null)
        };
    }
}
