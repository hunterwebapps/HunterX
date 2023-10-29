namespace HunterX.Trader.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected Entity(Guid? id = null)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id), "Id cannot be a default Guid.");
        }

        this.Id = id ?? Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Entity other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        return this.Id == other.Id;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + this.Id).GetHashCode();
    }
}
