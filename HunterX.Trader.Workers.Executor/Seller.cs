using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Workers.Executor;

public class Seller : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.Information("Seller running at {time}", DateTimeOffset.Now);
        }
    }
}
