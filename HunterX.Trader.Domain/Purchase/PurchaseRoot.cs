using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Purchase.Interfaces;

namespace HunterX.Trader.Domain.Purchase;

public class PurchaseRoot : AggregateRoot
{
    private readonly IBrokerService brokerService;

    public PurchaseRoot(IBrokerService brokerService)
        : this(null, brokerService)
    { }

    public PurchaseRoot(Guid? id, IBrokerService brokerService) : base(id)
    {
        this.brokerService = brokerService;
    }
}
