using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Workers.BackTester;

public class BackTester : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.Information("Back Testerrunning at {time}", DateTimeOffset.Now);
        }
    }
}
