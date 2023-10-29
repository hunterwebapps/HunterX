using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Workers.Executor;

public class Buyer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.Information("Buyer running at {time}", DateTimeOffset.Now);
        }
    }
}
