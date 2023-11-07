namespace HunterX.Trader.Domain.Common.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}
