namespace HunterX.Trader.Domain.Common.Interfaces.DomainEvents;

public interface IDomainEvent
{
    public System.DateTime Occurred { get; set; }
}