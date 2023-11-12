using HunterX.Trader.Domain.Common;

namespace HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

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
        Date = date;
        Open = open;
        Close = close;
        Exchange = exchange;
        Name = name;
        Created = created;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Date;
        yield return Open;
        yield return Close;
        yield return Exchange;
        yield return Name;
    }
}
