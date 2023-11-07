using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Workers.Executor;

public class SellerFeed : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.Information("Seller Feed running at {time}", DateTimeOffset.Now);
        }
    }
}
