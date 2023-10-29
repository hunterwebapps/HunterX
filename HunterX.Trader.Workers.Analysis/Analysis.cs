using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Workers.Analysis;

public class Analysis : BackgroundService
{
    private readonly AnalysisManager analysisManager;

    public Analysis(AnalysisManager analysisManager)
    {
        this.analysisManager = analysisManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.Information("Analysis running at {time}", DateTimeOffset.Now);

            await this.analysisManager.AnalyzeStocksAsync();

            Logger.Information("Completed Analysis at {time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
