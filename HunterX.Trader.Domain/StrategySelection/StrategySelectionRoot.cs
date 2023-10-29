using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Strategies;
using HunterX.Trader.Domain.StrategySelection.ValueObjects;
using MarketAnalysis = HunterX.Trader.Domain.StrategySelection.Analysis.MarketAnalysis;
using TechnicalAnalysis = HunterX.Trader.Domain.StrategySelection.Analysis.TechnicalAnalysis;

namespace HunterX.Trader.Domain.StrategySelection;

public class StrategySelectionRoot : AggregateRoot
{
    private readonly TickerSymbol tickerSymbol;

    public StrategySelectionRoot(TickerSymbol tickerSymbol) : base(null)
    {
        this.tickerSymbol = tickerSymbol;
    }

    public IStrategy DetermineStrategy()
    {
        var marketAnalysis = new MarketAnalysis();
        var technicalAnalysis = new TechnicalAnalysis();

        return null;
    }
}
