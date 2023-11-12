using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Workers.Stocks;

public class OrderMonitor : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public OrderMonitor(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = this.serviceProvider.CreateAsyncScope();

        var orderManager = scope.ServiceProvider.GetRequiredService<OrderManager>();

        using var _ = Logger.AddScope("Worker", nameof(OrderMonitor));

        while (!stoppingToken.IsCancellationRequested)
        {
            await orderManager.SyncOrderDataAsync();

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
