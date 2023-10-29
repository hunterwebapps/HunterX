using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Strategies.Enum;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;

namespace HunterX.Trader.Domain.StrategySelection.Strategies.ValueObjects;

public class ExecutionDecision : ValueObject
{
    public TradeAction TradeAction { get; private set; }
    public TickerSymbol TickerSymbol { get; private set; }
    public int Quantity { get; set; }

    public ExecutionDecision(TradeAction tradeAction, TickerSymbol tickerSymbol, int quantity)
    {
        this.TradeAction = tradeAction;
        this.TickerSymbol = tickerSymbol;
        this.Quantity = quantity;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return this.TradeAction;
        yield return this.TickerSymbol;
        yield return this.Quantity;
    }
}
