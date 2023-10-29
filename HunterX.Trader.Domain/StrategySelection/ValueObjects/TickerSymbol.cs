using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Enums;

namespace HunterX.Trader.Domain.StrategySelection.ValueObjects;

public class TickerSymbol : ValueObject
{
    public string Ticker { get; }
    public string Name { get; }
    public string Exchange { get; set; }
    public MarketType Market { get; set; }
    public DateTime Created { get; set; }

    public TickerSymbol(string ticker, string name, string exchange, MarketType marketType, DateTime created)
    {
        if (string.IsNullOrWhiteSpace(ticker)) throw new ArgumentNullException(nameof(ticker));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(exchange)) throw new ArgumentNullException(nameof(exchange));
        if (marketType == MarketType.Unknown) throw new ArgumentNullException(nameof(marketType));

        this.Ticker = ticker;
        this.Name = name;
        this.Exchange = exchange;
        this.Market = marketType;
        this.Created = created;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return this.Ticker;
        yield return this.Name;
        yield return this.Exchange;
        yield return this.Market;
    }
}
      