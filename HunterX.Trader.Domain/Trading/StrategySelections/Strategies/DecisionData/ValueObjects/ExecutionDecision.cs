using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.Enum;

namespace HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;

public record ExecutionDecision
{
    private ExecutionDecision() { }

    public static ExecutionDecision MakeBuyDecision(string symbol, DateTime signalTime, decimal price, decimal stop, decimal limit, decimal stopLoss, TechnicalIndicators indicators)
    {
        return new ExecutionDecision()
        {
            Action = TradeAction.Buy,
            Symbol = symbol,
            SignalTime = signalTime,
            Price = price,
            Stop = stop,
            Limit = limit,
            StopLoss = stopLoss,
            Indicators = indicators,
        };
    }

    public static ExecutionDecision MakeSellDecision(string symbol, DateTime signalTime, TechnicalIndicators indicators)
    {
        return new ExecutionDecision()
        {
            Action = TradeAction.Sell,
            Symbol = symbol,
            SignalTime = signalTime,
            Indicators = indicators,
            Limit = null,
            Price = null,
            Stop = null,
            StopLoss = null,
        };
    }

    public static ExecutionDecision MakeNoDecision(string symbol, DateTime signalTime, TechnicalIndicators indicators)
    {
        return new ExecutionDecision()
        {
            Action = TradeAction.None,
            Symbol = symbol,
            SignalTime = signalTime,
            Indicators = indicators,
            Limit = null,
            Price = null,
            Stop = null,
            StopLoss = null,
        };
    }

    public bool IsOrderDecision =>
        this.Action == TradeAction.Buy
        || this.Action == TradeAction.Sell;

    public required string Symbol { get; init; }
    public required TradeAction Action { get; init; }
    public required DateTime? SignalTime { get; init; }
    public required decimal? Price { get; init; }
    public required decimal? Stop { get; init; }
    public required decimal? Limit { get; init; }
    public required decimal? StopLoss { get; init; }
    public required TechnicalIndicators Indicators { get; init; }
}
