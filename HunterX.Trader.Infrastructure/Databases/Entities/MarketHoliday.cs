namespace HunterX.Trader.Infrastructure.Databases.Entities;

public class MarketHoliday
{
    public DateOnly Date { get; set; }
    public TimeOnly? Open { get; set; }
    public TimeOnly? Close { get; set; }
    public string Exchange { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateTime Created { get; set; }
}


