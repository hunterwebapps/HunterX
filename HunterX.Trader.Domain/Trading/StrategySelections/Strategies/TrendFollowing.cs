using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.Enum;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;
using Skender.Stock.Indicators;

namespace HunterX.Trader.Domain.Trading.StrategySelections.Strategies;

public class TrendFollowing : Entity, IStrategy
{
    private readonly Asset stock;
    private readonly List<Bar> bars;

    private readonly List<ExecutionDecision> executionDecisions = new();

    public TrendFollowing(Asset stock, IReadOnlyList<Bar> bars)
    {
        this.stock = stock;
        this.bars = bars.ToList();
    }

    public string Name => nameof(TrendFollowing);

    private bool IsPositionOpen => this.executionDecisions.LastOrDefault()?.Action == TradeAction.Buy;

    private TechnicalIndicators GetIndicators() => new()
    {
        PivotPoints = this.bars.GetPivotPoints(PeriodSize.OneHour, PivotPointType.Fibonacci).ToList(),
        Pivots = this.bars.GetPivots(leftSpan: 2, rightSpan: 2, maxTrendPeriods: 50, endType: EndType.HighLow).ToList(),
        RollingPivots = this.bars.GetRollingPivots(windowPeriods: 10, offsetPeriods: 5, pointType: PivotPointType.Fibonacci).ToList(),
        ShortEMA = this.bars.GetEma(9).ToList(),
        LongEMA = this.bars.GetEma(20).ToList(),
        MovingAverageConvergenceDivergence = this.bars.GetMacd(fastPeriods: 12, slowPeriods: 26, signalPeriods: 9).ToList(),
        VolumeWeightedAvgPrice = this.bars.GetVwma(9).ToList(),
        RelativeStrengthIndex = this.bars.GetRsi(12).ToList(),
        AverageTrueRange = this.bars.GetAtr(14).ToList(),
    };

    public IReadOnlyList<Bar> Bars => this.bars;

    public Asset Stock => this.stock;

    public decimal DetermineViabilityRate()
    {
        return 0.75m;
    }

    public ExecutionDecision EvaluateLatestBar(Bar bar)
    {
        this.bars.Add(bar);

        var indicators = GetIndicators();

        var signal = this.IsPositionOpen
            ? GetExitSignal(indicators, this.bars, this.executionDecisions.Last())
            : GetEntrySignal(indicators, this.bars);

        var signalTime = this.bars[^1].Date;

        ExecutionDecision decision;
        if (signal == TradeAction.Sell)
        {
            decision = ExecutionDecision.MakeSellDecision(this.stock.Symbol, signalTime, indicators);
        }
        else if (signal == TradeAction.Buy)
        {
            var price = this.bars[^1].Close;
            var stop = decimal.Round(price * 0.9m, 2, MidpointRounding.AwayFromZero);
            var limit = decimal.Round(price * 1.1m, 2, MidpointRounding.AwayFromZero);
            decision = ExecutionDecision.MakeBuyDecision(this.stock.Symbol, signalTime, price, stop, limit, stopLoss: stop, indicators);
        }
        else
        {
            decision = ExecutionDecision.MakeNoDecision(this.stock.Symbol, signalTime, indicators);
        }

        this.executionDecisions.Add(decision);

        return decision;
    }

    /// <summary>
    /// Step 3: Define Entry Criteria
    /// A potential buy signal is triggered when:
    /// The 9-minute EMA crosses above the 21-minute EMA, indicating a potential uptrend.
    /// The MACD line crosses above its signal line, indicating a momentum shift to the upside.
    /// RSI is above a threshold(e.g., 50) but not overbought(e.g., not above 70), suggesting upward momentum.
    /// The price is above the VWAP, indicating the stock is trading higher than the day's average valuation.
    /// </summary>
    /// <returns></returns>
    private static TradeAction GetEntrySignal(TechnicalIndicators indicators, IReadOnlyList<Bar> bars)
    {
        var isEMACrossedAbove = GetEMACrossedDirection(indicators) == Direction.Up;
        var isMACDUpwardMomentum = indicators.MovingAverageConvergenceDivergence[^1].Macd > indicators.MovingAverageConvergenceDivergence[^1].Signal;
        var isUpwardMomentum = indicators.RelativeStrengthIndex[^1].Rsi > 50
            && indicators.RelativeStrengthIndex[^1].Rsi < 70;
        var isPriceAboveVWAP = indicators.VolumeWeightedAvgPrice[^1].Vwma < (double)bars[^1].Close;

        // TODO: Determine bullish price action patterns
        /**
        Step 5: Check for Bullish Price Action Patterns
        Look for bullish price action patterns that suggest an upward move:

        Hammer candlestick pattern after a downtrend.
        Bullish engulfing pattern.
        Breakout above a recent resistance level with high volume.
         */

        if (isEMACrossedAbove
            && isMACDUpwardMomentum
            && isUpwardMomentum
            && isPriceAboveVWAP)
        {
            return TradeAction.Buy;
        }

        return TradeAction.None;
    }

    /// <summary>
    /// Step 4: Define Exit Criteria
    /// A sell signal is not fixed but is based on a trailing stop loss:
    /// If the 9 - minute EMA crosses below the 21 - minute EMA, start tracking the stop - loss.
    /// Set the initial stop - loss at a recent swing low or a multiple of the ATR below the current price(e.g., 2xATR).
    /// Update the stop - loss level if the price makes a new high and the stop - loss according to the new support level is higher than the previous stop - loss.
    /// </summary>
    /// <returns></returns>
    private static TradeAction GetExitSignal(TechnicalIndicators indicators, IReadOnlyList<Bar> bars, ExecutionDecision lastBuyDecision)
    {
        var isEMACrossedBelow = GetEMACrossedDirection(indicators) == Direction.Down;
        var isMACDDownwardMomentum = indicators.MovingAverageConvergenceDivergence[^1].Macd < indicators.MovingAverageConvergenceDivergence[^1].Signal;
        var isAtStopLoss = bars[^1].Close < lastBuyDecision.StopLoss;

        if (isAtStopLoss
            || isEMACrossedBelow && isMACDDownwardMomentum)
        {
            return TradeAction.Sell;
        }

        return TradeAction.None;
    }

    private static Direction? GetEMACrossedDirection(TechnicalIndicators indicators)
    {
        if (indicators.ShortEMA.Count < 2 || indicators.LongEMA.Count < 2)
        {
            return null;
        }

        var previousShortEMA = indicators.ShortEMA[^2];
        var previousLongEMA = indicators.LongEMA[^2];
        var currentShortEMA = indicators.ShortEMA[^1];
        var currentLongEMA = indicators.LongEMA[^1];

        if (previousShortEMA.Ema < previousLongEMA.Ema
            && currentShortEMA.Ema > currentLongEMA.Ema)
        {
            return Direction.Up;
        }
        else if (currentShortEMA.Ema > previousShortEMA.Ema
            && currentLongEMA.Ema < previousLongEMA.Ema)
        {
            return Direction.Down;
        }

        return null;
    }
}
