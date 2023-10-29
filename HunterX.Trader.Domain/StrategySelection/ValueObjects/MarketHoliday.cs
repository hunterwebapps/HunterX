using HunterX.Trader.Domain.Common;

namespace HunterX.Trader.Domain.StrategySelection.ValueObjects;

public class MarketHoliday : ValueObject
{
    public DateOnly Date { get; private set; }
    public TimeOnly? Open { get; private set; }
    public TimeOnly? Close { get; private set; }
    public string Exchange { get; private set; }
    public string Name { get; set; }
    public bool Closed => Open == null && Close == null;
    public DateTime Created { get; set; }

    public MarketHoliday(DateOnly date, TimeOnly? open, TimeOnly? close, string exchange, string name, DateTime created)
    {
        this.Date = date;
        this.Open = open;
        this.Close = close;
        this.Exchange = exchange;
        this.Name = name;
        this.Created = created;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return this.Date;
        yield return this.Open;
        yield return this.Close;
        yield return this.Exchange;
        yield return this.Name;
    }
}
