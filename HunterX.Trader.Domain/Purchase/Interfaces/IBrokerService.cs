using HunterX.Trader.Domain.Purchase.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;

namespace HunterX.Trader.Domain.Purchase.Interfaces;

public interface IBrokerService
{
    Task<decimal> GetBuyingPowerAsync();
    Task<Order> ExecuteBuyOrderAsync(ExecutionDecision executionDecision, int quantity);
    Task<Order> ExecuteSellOrderAsync(ExecutionDecision executionDecision, int quantity);
}
