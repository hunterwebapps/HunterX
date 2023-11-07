using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.Enum;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using Skender.Stock.Indicators;

namespace HunterX.Trader.Domain.StrategySelection.Strategies;

public class TrendFollowing : Entity, IStrategy
{
    private readonly StockBasics stockBasics;
    private readonly IReadOnlyList<ChartData> bars;
    private readonly IDateTimeProvider dateTimeProvider;

    private readonly TechnicalIndicators indicators;

    public TrendFollowing(StockBasics stockBasics, IReadOnlyList<ChartData> bars, IDateTimeProvider dateTimeProvider)
    {
        this.stockBasics = stockBasics;
        this.bars = bars;
        this.dateTimeProvider = dateTimeProvider;
        this.indicators = new TechnicalIndicators()
        {
            PivotPoints = this.bars.GetPivotPoints(PeriodSize.OneHour, PivotPointType.Fibonacci).ToList(),
            Pivots = this.bars.GetPivots(leftSpan: 2, rightSpan: 2, maxTrendPeriods: 20, endType: EndType.HighLow).ToList(),
            RollingPivots = this.bars.GetRollingPivots(windowPeriods: 10, offsetPeriods: 5, pointType: PivotPointType.Fibonacci).ToList(),
            ShortEMA = this.bars.GetEma(9).ToList(),
            LongEMA = this.bars.GetEma(20).ToList(),
            MovingAverageConvergenceDivergence = this.bars.GetMacd(fastPeriods: 12, slowPeriods: 26, signalPeriods: 9).ToList(),
            VolumeWeightedAvgPrice = this.bars.GetVwma(9).ToList(),
            RelativeStrengthIndex = this.bars.GetRsi(12).ToList(),
            AverageTrueRange = this.bars.GetAtr(14).ToList(),
        };
    }

    public string Name => nameof(TrendFollowing);

    // TODO: Implement the following
    /**
     * Step 4: Determine Entry Criteria
        Define criteria for a potential entry signal for an upward trend:

        The 9-period EMA crosses above the 20-period EMA, indicating a potential short-term upward trend.
        The MACD line crosses above its signal line, indicating a momentum shift to the upside.
        RSI is rising and below 70 but above 50, indicating increasing bullish momentum without being overbought.
        A recent increase in volume, which can confirm the strength of the trend.
        Step 5: Check for Bullish Price Action Patterns
        Look for bullish price action patterns that suggest an upward move:

        Hammer candlestick pattern after a downtrend.
        Bullish engulfing pattern.
        Breakout above a recent resistance level with high volume.
     */

    public ExecutionDecisionDetails DetermineBuyDecision()
    {
        var (signal, signalTime) = GetEntrySignal();

        var decision = new ExecutionDecision()
        {
            Symbol = this.stockBasics.Symbol,
            Action = signal,
            SignalTime = signalTime,
            Price = this.bars[this.bars.Count - 1].Close,
            Stop = CalculateStopLoss(),
        };

        return new ExecutionDecisionDetails()
        {
            ExecutionDecision = decision,
            StockBasics = this.stockBasics,
            StrategyName = this.Name,
            Bars = this.bars,
            Indicators = this.indicators,
            CreatedAt = this.dateTimeProvider.Now.DateTime,
        };
    }

    public ExecutionDecisionDetails DetermineSellDecision()
    {
        var (signal, signalTime) = GetExitSignal();

        var decision = new ExecutionDecision()
        {
            Symbol = this.stockBasics.Symbol,
            Action = signal,
            SignalTime = signalTime,
            Price = this.bars[this.bars.Count - 1].Close,
            Stop = CalculateStopLoss(),
        };

        return new ExecutionDecisionDetails()
        {
            ExecutionDecision = decision,
            StockBasics = this.stockBasics,
            StrategyName = this.Name,
            Bars = this.bars,
            Indicators = this.indicators,
            CreatedAt = this.dateTimeProvider.Now.DateTime,
        };
    }

    /// <summary>
    /// Step 3: Define Entry Criteria
    /// A potential buy signal is triggered when:
    /// The 9-minute EMA crosses above the 21-minute EMA, indicating a potential uptrend.
    /// RSI is above a threshold(e.g., 50) but not overbought(e.g., not above 70), suggesting upward momentum.
    /// The price is above the VWAP, indicating the stock is trading higher than the day's average valuation.
    /// </summary>
    /// <returns></returns>
    private (TradeAction, DateTime?) GetEntrySignal()
    {
        if (this.indicators.ShortEMA.Count == 0)
        {
            return (TradeAction.None, null);
        }

        var lastNineMinuteEMA = this.indicators.ShortEMA[this.indicators.ShortEMA.Count - 1];
        var lastTwentyOneMinuteEMA = this.indicators.LongEMA[this.indicators.LongEMA.Count - 1];
        if (lastNineMinuteEMA.Ema < lastTwentyOneMinuteEMA.Ema)
        {
            return (TradeAction.None, null);
        }

        var previousNineMinuteEMA = lastNineMinuteEMA;
        var previousTwentyOneMinuteEMA = lastTwentyOneMinuteEMA;
        for (var i = this.indicators.ShortEMA.Count - 2; i >= 0; i--)
        {
            var currentShortEMA = this.indicators.ShortEMA[i];
            var currentLongEMA = this.indicators.LongEMA[i];

            if (previousNineMinuteEMA.Ema > previousTwentyOneMinuteEMA.Ema
                && currentShortEMA.Ema < currentLongEMA.Ema)
            {
                if (this.indicators.RelativeStrengthIndex[i].Rsi > 50
                && this.indicators.RelativeStrengthIndex[i].Rsi < 70
                && this.indicators.VolumeWeightedAvgPrice[i].Vwma > (double)this.bars[i].Close)
                {
                    return (TradeAction.Buy, currentShortEMA.Date);
                }
                else
                {
                    return (TradeAction.None, null);
                }
            }
        }

        return (TradeAction.None, null);
    }

    /// <summary>
    /// Step 4: Define Exit Criteria
    /// A sell signal is not fixed but is based on a trailing stop loss:
    /// If the 9 - minute EMA crosses below the 21 - minute EMA, start tracking the stop - loss.
    /// Set the initial stop - loss at a recent swing low or a multiple of the ATR below the current price(e.g., 2xATR).
    /// Update the stop - loss level if the price makes a new high and the stop - loss according to the new support level is higher than the previous stop - loss.
    /// </summary>
    /// <returns></returns>
    private (TradeAction, DateTime?) GetExitSignal()
    {
        var lastNineMinuteEMA = this.indicators.ShortEMA[this.indicators.ShortEMA.Count - 1];
        var lastTwentyOneMinuteEMA = this.indicators.LongEMA[this.indicators.LongEMA.Count - 1];
        if (lastNineMinuteEMA.Ema > lastTwentyOneMinuteEMA.Ema)
        {
            return (TradeAction.None, null);
        }

        var previousNineMinuteEMA = lastNineMinuteEMA;
        var previousTwentyOneMinuteEMA = lastTwentyOneMinuteEMA;
        for (var i = this.indicators.ShortEMA.Count - 2; i >= 0; i--)
        {
            var currentShortEMA = this.indicators.ShortEMA[i];
            var currentLongEMA = this.indicators.LongEMA[i];

            if (previousNineMinuteEMA.Ema < previousTwentyOneMinuteEMA.Ema
                && currentShortEMA.Ema > currentLongEMA.Ema)
            {
                if (this.indicators.RelativeStrengthIndex[i].Rsi < 50
                    && this.indicators.RelativeStrengthIndex[i].Rsi > 30
                    && this.indicators.VolumeWeightedAvgPrice[i].Vwma < (double)this.bars[i].Close)
                {
                    return (TradeAction.Sell, currentShortEMA.Date);
                }
                else
                {
                    return (TradeAction.None, null);
                }
            }
        }

        return (TradeAction.None, null);
    }

    private decimal CalculateStopLoss()
    {
        return decimal.Round(this.bars[this.bars.Count - 1].Close * 0.95m, 2, MidpointRounding.AwayFromZero);
    }
}
