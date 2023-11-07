namespace HunterX.Trader.Infrastructure.Messaging.Configuration;

public class ConsumerSettings
{
    public required Type Consumer { get; set; }
    public int ConcurrencyLimit { get; set; } = 1;
}
