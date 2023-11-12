using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Trading.Purchases.Interfaces;

namespace HunterX.Trader.Domain.Trading.Purchases;

public class Purchase : AggregateRoot
{
    private readonly IBrokerService brokerService;

    public Purchase(IBrokerService brokerService)
        : this(null, brokerService)
    { }

    public Purchase(Guid? id, IBrokerService brokerService) : base(id)
    {
        this.brokerService = brokerService;
    }
}
