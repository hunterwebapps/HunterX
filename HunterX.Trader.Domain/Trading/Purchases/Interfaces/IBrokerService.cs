using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.ValueObjects;

namespace HunterX.Trader.Domain.Trading.Purchases.Interfaces;

public interface IBrokerService
{
    Task<decimal> GetBuyingPowerAsync();
    Task<Position> GetPositionAsync(string symbol);
    Task<IReadOnlyList<Position>> GetOpenPositionsAsync();
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<IReadOnlyList<Order>> GetOpenOrdersAsync();
    Task<Order> ExecuteOrderAsync(ExecutionDecision executionDecision, int quantity);
}
