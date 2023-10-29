namespace HunterX.Trader.Domain.Common.Interfaces.DomainEvents;

public interface IDomainEvent
{
    public DateTime Occurred { get; set; }
}