using HunterX.Trader.Domain.Common.Interfaces.DomainEvents;

namespace HunterX.Trader.Domain.Common;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> domainEvents = new();
    internal IReadOnlyList<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    protected AggregateRoot(Guid? id) : base(id)
    { }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        this.domainEvents.Add(domainEvent);
    }

    protected void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        this.domainEvents.Remove(domainEvent);
    }

    protected void ClearDomainEvents()
    {
        this.domainEvents.Clear();
    }
}
