using Alpaca.Markets;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Trading.Purchases.Interfaces;
using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.Enum;
using HunterX.Trader.Domain.Trading.ValueObjects;

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

    public async Task<Position> GetPositionAsync(string symbol)
    {
        var position = await this.tradingClient.GetPositionAsync(symbol);

        return MakePosition(position);
    }

    public async Task<IReadOnlyList<Position>> GetOpenPositionsAsync()
    {
        var positions = await this.tradingClient.ListPositionsAsync();

        return positions
            .Select(MakePosition)
            .ToList();
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        var order = await this.tradingClient.GetOrderAsync(orderId);

        return MakeOrder(order);
    }

    public async Task<IReadOnlyList<Order>> GetOpenOrdersAsync()
    {
        var orders = await this.tradingClient.ListOrdersAsync(new ListOrdersRequest()
        {
            OrderStatusFilter = OrderStatusFilter.Open,
            RollUpNestedOrders = true,
        });

        return orders.Select(MakeOrder).ToList();
    }

    public async Task<Order> ExecuteOrderAsync(ExecutionDecision executionDecision, int quantity)
    {
        if (executionDecision.Action == TradeAction.None)
        {
            throw new ArgumentException($"Must have a {nameof(TradeAction)} ({executionDecision.Action}) to execute order.");
        }

        var orderId = Guid.NewGuid();

        using var s1 = Logger.AddScope("Execution Decision", executionDecision, destructureObject: true);
        using var s2 = Logger.AddScope("Order Id", orderId);

        OrderBase orderRequest;
        
        if (executionDecision.Action == TradeAction.Buy)
        {
            orderRequest = OrderSide.Buy
                .StopLimit(
                    executionDecision.Symbol,
                    OrderQuantity.FromInt64(quantity),
                    executionDecision.Stop!.Value,
                    executionDecision.Limit!.Value)
                .StopLoss(executionDecision.StopLoss!.Value)
                .WithDuration(TimeInForce.Day)
                .WithClientOrderId(orderId.ToString());
        }
        else
        {
            orderRequest = OrderSide.Sell
                .Market(executionDecision.Symbol, OrderQuantity.FromInt64(quantity))
                .WithDuration(TimeInForce.Gtc)
                .WithClientOrderId(orderId.ToString());
        }

        Logger.Information("Submitting Order to Alpaca API {@orderRequest}", orderRequest);

        var submittedOrder = await this.tradingClient.PostOrderAsync(orderRequest);

        Logger.Information("Successfully Executed Order. {@order}", submittedOrder);

        var orderEntity = MakeOrder(submittedOrder);

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

    private static Order MakeOrder(IOrder order)
    {
        return new Order()
        {
            OrderId = order.OrderId,
            Symbol = order.Symbol,
            TimeInForce = MakeTimeInForce(order.TimeInForce),
            AssetClass = MakeAssetClass(order.AssetClass),
            FilledPrice = order.AverageFillPrice,
            CreatedAt = order.CreatedAtUtc!.Value,
            LimitPrice = order.LimitPrice,
            OrderSide = MakeOrderSide(order.OrderSide),
            OrderType = MakeOrderType(order.OrderType),
            Quantity = (int)order.IntegerQuantity,
            StopOrderPrice = order.StopPrice,
            TrailPercent = order.TrailOffsetInPercent,
            TrailPrice = order.TrailOffsetInDollars,
            CancelledAt = order.CancelledAtUtc,
            ExpiredAt = order.ExpiredAtUtc,
            FailedAt = order.FailedAtUtc,
            FilledAt = order.FilledAtUtc,
            UpdatedAt = order.UpdatedAtUtc,
            Legs = order.Legs.Select(MakeOrder).ToList(),
        };
    }

    private static Position MakePosition(IPosition position)
    {
        return new Position(position.AssetId)
        {
            Symbol = position.Symbol,
            Quantity = (int)position.IntegerQuantity,
            AverageEntryPrice = position.AverageEntryPrice,
            AssetClass = MakeAssetClass(position.AssetClass),
            Side = MakePositionSide(position.Side),
            MarketValue = position.MarketValue,
        };
    }

    private static Domain.Common.Enums.AssetClass MakeAssetClass(AssetClass assetClass)
    {
        return assetClass switch
        {
            AssetClass.UsEquity => Domain.Common.Enums.AssetClass.Stocks,
            AssetClass.Crypto => Domain.Common.Enums.AssetClass.Crypto,
            _ => throw new ArgumentOutOfRangeException(nameof(assetClass), assetClass, null),
        };
    }

    private static Domain.Trading.Purchases.Enums.OrderSide MakeOrderSide(OrderSide orderSide)
    {
        return orderSide switch
        {
            OrderSide.Buy => Domain.Trading.Purchases.Enums.OrderSide.Buy,
            OrderSide.Sell => Domain.Trading.Purchases.Enums.OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null),
        };
    }

    private static Domain.Trading.Purchases.Enums.OrderType MakeOrderType(OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Market => Domain.Trading.Purchases.Enums.OrderType.Market,
            OrderType.Stop => Domain.Trading.Purchases.Enums.OrderType.Stop,
            OrderType.Limit => Domain.Trading.Purchases.Enums.OrderType.Limit,
            OrderType.StopLimit => Domain.Trading.Purchases.Enums.OrderType.StopLimit,
            OrderType.TrailingStop => Domain.Trading.Purchases.Enums.OrderType.TrailingStop,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null),
        };
    }

    private static Domain.Trading.Purchases.Enums.TimeInForce MakeTimeInForce(TimeInForce timeInForce)
    {
        return timeInForce switch
        {
            TimeInForce.Day => Domain.Trading.Purchases.Enums.TimeInForce.Day,
            TimeInForce.Gtc => Domain.Trading.Purchases.Enums.TimeInForce.GoodTilCancel,
            TimeInForce.Opg => Domain.Trading.Purchases.Enums.TimeInForce.MarketOpen,
            TimeInForce.Fok => Domain.Trading.Purchases.Enums.TimeInForce.FillOrKill,
            TimeInForce.Cls => Domain.Trading.Purchases.Enums.TimeInForce.MarketClose,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null),
        };
    }

    private static Domain.Trading.Enums.PositionSide MakePositionSide(PositionSide position)
    {
        return position switch
        {
            PositionSide.Long => Domain.Trading.Enums.PositionSide.Long,
            PositionSide.Short => Domain.Trading.Enums.PositionSide.Short,
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null),
        };
    }
}
