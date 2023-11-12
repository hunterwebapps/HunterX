using AutoMapper;
using HunterX.Trader.Infrastructure.Databases.Entities.Enums;

namespace HunterX.Trader.Infrastructure.Databases.Entities.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, Domain.Trading.Purchases.ValueObjects.Order>()
            .ConvertUsing((order) => MakeDomainOrder(order));

        CreateMap<Domain.Trading.Purchases.ValueObjects.Order, Order>()
            .ConvertUsing((order) => MakeInfrastructureOrder(order, null));
    }

    private static Domain.Trading.Purchases.ValueObjects.Order MakeDomainOrder(Order order)
    {
        return new Domain.Trading.Purchases.ValueObjects.Order()
        {
            OrderId = order.OrderId,
            Symbol = order.Symbol,
            TimeInForce = MakeDomainTimeInForce(order.TimeInForce),
            AssetClass = MakeDomainAssetClass(order.AssetClass),
            FilledPrice = order.FilledPrice,
            CreatedAt = order.CreatedAt,
            LimitPrice = order.LimitPrice,
            OrderSide = MakeDomainOrderSide(order.OrderSide),
            OrderType = MakeDomainOrderType(order.OrderType),
            Quantity = order.Quantity,
            StopOrderPrice = order.StopOrderPrice,
            TrailPercent = order.TrailPercent,
            TrailPrice = order.TrailPrice,
            CancelledAt = order.CancelledAt,
            ExpiredAt = order.ExpiredAt,
            FailedAt = order.FailedAt,
            FilledAt = order.FilledAt,
            UpdatedAt = order.UpdatedAt,
            Legs = order.Legs
                .Select(MakeDomainOrder)
                .ToList(),
        };
    }

    private static Domain.Common.Enums.AssetClass MakeDomainAssetClass(AssetClass assetClass)
    {
        return assetClass switch
        {
            AssetClass.Stocks => Domain.Common.Enums.AssetClass.Stocks,
            AssetClass.Crypto => Domain.Common.Enums.AssetClass.Crypto,
            _ => throw new ArgumentOutOfRangeException(nameof(assetClass), assetClass, null)
        };
    }

    private static Domain.Trading.Purchases.Enums.OrderSide MakeDomainOrderSide(OrderSide orderSide)
    {
        return orderSide switch
        {
            OrderSide.Buy => Domain.Trading.Purchases.Enums.OrderSide.Buy,
            OrderSide.Sell => Domain.Trading.Purchases.Enums.OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null)
        };
    }

    private static Domain.Trading.Purchases.Enums.OrderType MakeDomainOrderType(OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Market => Domain.Trading.Purchases.Enums.OrderType.Market,
            OrderType.Stop => Domain.Trading.Purchases.Enums.OrderType.Stop,
            OrderType.Limit => Domain.Trading.Purchases.Enums.OrderType.Limit,
            OrderType.StopLimit => Domain.Trading.Purchases.Enums.OrderType.StopLimit,
            OrderType.TrailingStop => Domain.Trading.Purchases.Enums.OrderType.TrailingStop,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }

    private static Domain.Trading.Purchases.Enums.TimeInForce MakeDomainTimeInForce(TimeInForce timeInForce)
    {
        return timeInForce switch
        {
            TimeInForce.Day => Domain.Trading.Purchases.Enums.TimeInForce.Day,
            TimeInForce.GoodTilCancel => Domain.Trading.Purchases.Enums.TimeInForce.GoodTilCancel,
            TimeInForce.MarketOpen => Domain.Trading.Purchases.Enums.TimeInForce.MarketOpen,
            TimeInForce.FillOrKill => Domain.Trading.Purchases.Enums.TimeInForce.FillOrKill,
            TimeInForce.MarketClose => Domain.Trading.Purchases.Enums.TimeInForce.MarketClose,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null)
        };
    }

    private static Order MakeInfrastructureOrder(Domain.Trading.Purchases.ValueObjects.Order order, Guid? parentOrderId)
    {
        return new Order()
        {
            OrderId = order.OrderId,
            ParentOrderId = parentOrderId,
            Symbol = order.Symbol,
            TimeInForce = MakeInfrastructureTimeInForce(order.TimeInForce),
            AssetClass = MakeInfrastructureAssetClass(order.AssetClass),
            FilledPrice = order.FilledPrice,
            CreatedAt = order.CreatedAt,
            LimitPrice = order.LimitPrice,
            OrderSide = MakeInfrastructureOrderSide(order.OrderSide),
            OrderType = MakeInfrastructureOrderType(order.OrderType),
            Quantity = order.Quantity,
            StopOrderPrice = order.StopOrderPrice,
            TrailPercent = order.TrailPercent,
            TrailPrice = order.TrailPrice,
            CancelledAt = order.CancelledAt,
            ExpiredAt = order.ExpiredAt,
            FailedAt = order.FailedAt,
            FilledAt = order.FilledAt,
            UpdatedAt = order.UpdatedAt,
            Legs = order.Legs
                .Select((leg) => MakeInfrastructureOrder(leg, order.OrderId))
                .ToList(),
        };
    }

    private static AssetClass MakeInfrastructureAssetClass(Domain.Common.Enums.AssetClass assetClass)
    {
        return assetClass switch
        {
            Domain.Common.Enums.AssetClass.Stocks => AssetClass.Stocks,
            Domain.Common.Enums.AssetClass.Crypto => AssetClass.Crypto,
            _ => throw new ArgumentOutOfRangeException(nameof(assetClass), assetClass, null)
        };
    }

    private static OrderSide MakeInfrastructureOrderSide(Domain.Trading.Purchases.Enums.OrderSide orderSide)
    {
        return orderSide switch
        {
            Domain.Trading.Purchases.Enums.OrderSide.Buy => OrderSide.Buy,
            Domain.Trading.Purchases.Enums.OrderSide.Sell => OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null)
        };
    }

    private static OrderType MakeInfrastructureOrderType(Domain.Trading.Purchases.Enums.OrderType orderType)
    {
        return orderType switch
        {
            Domain.Trading.Purchases.Enums.OrderType.Market => OrderType.Market,
            Domain.Trading.Purchases.Enums.OrderType.Stop => OrderType.Stop,
            Domain.Trading.Purchases.Enums.OrderType.Limit => OrderType.Limit,
            Domain.Trading.Purchases.Enums.OrderType.StopLimit => OrderType.StopLimit,
            Domain.Trading.Purchases.Enums.OrderType.TrailingStop => OrderType.TrailingStop,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }

    private static TimeInForce MakeInfrastructureTimeInForce(Domain.Trading.Purchases.Enums.TimeInForce timeInForce)
    {
        return timeInForce switch
        {
            Domain.Trading.Purchases.Enums.TimeInForce.Day => TimeInForce.Day,
            Domain.Trading.Purchases.Enums.TimeInForce.GoodTilCancel => TimeInForce.GoodTilCancel,
            Domain.Trading.Purchases.Enums.TimeInForce.MarketOpen => TimeInForce.MarketOpen,
            Domain.Trading.Purchases.Enums.TimeInForce.FillOrKill => TimeInForce.FillOrKill,
            Domain.Trading.Purchases.Enums.TimeInForce.MarketClose => TimeInForce.MarketClose,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null)
        };
    }
}
