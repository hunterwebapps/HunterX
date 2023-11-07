using AutoMapper;
using HunterX.Trader.Infrastructure.Databases.Entities;
using HunterX.Trader.Infrastructure.Databases.Entities.Enums;

namespace HunterX.Trader.Infrastructure.Databases.Entities.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, Domain.Purchase.ValueObjects.Order>()
            .ConvertUsing((order) => MakeDomainOrder(order));

        CreateMap<Domain.Purchase.ValueObjects.Order, Order>()
            .ConvertUsing((order) => MakeInfrastructureOrder(order, null));
    }

    private static Domain.Purchase.ValueObjects.Order MakeDomainOrder(Order order)
    {
        return new Domain.Purchase.ValueObjects.Order()
        {
            OrderId = order.OrderId,
            Symbol = order.Symbol,
            TimeInForce = MakeDomainTimeInForce(order.TimeInForce),
            AssetClass = MakeDomainAssetClass(order.AssetClass),
            OrderedPrice = order.OrderedPrice,
            FilledPrice = order.FilledPrice,
            CreatedAt = order.CreatedAt,
            LimitPrice = order.LimitPrice,
            OrderSide = MakeDomainOrderSide(order.OrderSide),
            OrderType = MakeDomainOrderType(order.OrderType),
            Quantity = order.Quantity,
            StopOrderPrice = order.StopOrderPrice,
            StopLossPrice = order.StopLossPrice,
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

    private static Domain.Purchase.Enums.AssetClass MakeDomainAssetClass(AssetClass assetClass)
    {
        return assetClass switch
        {
            AssetClass.Stocks => Domain.Purchase.Enums.AssetClass.Stocks,
            AssetClass.Crypto => Domain.Purchase.Enums.AssetClass.Crypto,
            _ => throw new ArgumentOutOfRangeException(nameof(assetClass), assetClass, null)
        };
    }

    private static Domain.Purchase.Enums.OrderSide MakeDomainOrderSide(OrderSide orderSide)
    {
        return orderSide switch
        {
            OrderSide.Buy => Domain.Purchase.Enums.OrderSide.Buy,
            OrderSide.Sell => Domain.Purchase.Enums.OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null)
        };
    }

    private static Domain.Purchase.Enums.OrderType MakeDomainOrderType(OrderType orderType)
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

    private static Domain.Purchase.Enums.TimeInForce MakeDomainTimeInForce(TimeInForce timeInForce)
    {
        return timeInForce switch
        {
            TimeInForce.Day => Domain.Purchase.Enums.TimeInForce.Day,
            TimeInForce.GoodTilCancel => Domain.Purchase.Enums.TimeInForce.GoodTilCancel,
            TimeInForce.MarketOpen => Domain.Purchase.Enums.TimeInForce.MarketOpen,
            TimeInForce.FillOrKill => Domain.Purchase.Enums.TimeInForce.FillOrKill,
            TimeInForce.MarketClose => Domain.Purchase.Enums.TimeInForce.MarketClose,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null)
        };
    }

    private static Order MakeInfrastructureOrder(Domain.Purchase.ValueObjects.Order order, Guid? parentOrderId)
    {
        return new Order()
        {
            OrderId = order.OrderId,
            ParentOrderId = parentOrderId,
            Symbol = order.Symbol,
            TimeInForce = MakeInfrastructureTimeInForce(order.TimeInForce),
            AssetClass = MakeInfrastructureAssetClass(order.AssetClass),
            OrderedPrice = order.OrderedPrice,
            FilledPrice = order.FilledPrice,
            CreatedAt = order.CreatedAt,
            LimitPrice = order.LimitPrice,
            OrderSide = MakeInfrastructureOrderSide(order.OrderSide),
            OrderType = MakeInfrastructureOrderType(order.OrderType),
            Quantity = order.Quantity,
            StopOrderPrice = order.StopOrderPrice,
            StopLossPrice = order.StopLossPrice,
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

    private static AssetClass MakeInfrastructureAssetClass(Domain.Purchase.Enums.AssetClass assetClass)
    {
        return assetClass switch
        {
            Domain.Purchase.Enums.AssetClass.Stocks => AssetClass.Stocks,
            Domain.Purchase.Enums.AssetClass.Crypto => AssetClass.Crypto,
            _ => throw new ArgumentOutOfRangeException(nameof(assetClass), assetClass, null)
        };
    }

    private static OrderSide MakeInfrastructureOrderSide(Domain.Purchase.Enums.OrderSide orderSide)
    {
        return orderSide switch
        {
            Domain.Purchase.Enums.OrderSide.Buy => OrderSide.Buy,
            Domain.Purchase.Enums.OrderSide.Sell => OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null)
        };
    }

    private static OrderType MakeInfrastructureOrderType(Domain.Purchase.Enums.OrderType orderType)
    {
        return orderType switch
        {
            Domain.Purchase.Enums.OrderType.Market => OrderType.Market,
            Domain.Purchase.Enums.OrderType.Stop => OrderType.Stop,
            Domain.Purchase.Enums.OrderType.Limit => OrderType.Limit,
            Domain.Purchase.Enums.OrderType.StopLimit => OrderType.StopLimit,
            Domain.Purchase.Enums.OrderType.TrailingStop => OrderType.TrailingStop,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }

    private static TimeInForce MakeInfrastructureTimeInForce(Domain.Purchase.Enums.TimeInForce timeInForce)
    {
        return timeInForce switch
        {
            Domain.Purchase.Enums.TimeInForce.Day => TimeInForce.Day,
            Domain.Purchase.Enums.TimeInForce.GoodTilCancel => TimeInForce.GoodTilCancel,
            Domain.Purchase.Enums.TimeInForce.MarketOpen => TimeInForce.MarketOpen,
            Domain.Purchase.Enums.TimeInForce.FillOrKill => TimeInForce.FillOrKill,
            Domain.Purchase.Enums.TimeInForce.MarketClose => TimeInForce.MarketClose,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null)
        };
    }
}
