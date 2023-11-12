using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Workers.Stocks;

public class StockTrader : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public StockTrader(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = this.serviceProvider.CreateAsyncScope();

        var marketDataManager = scope.ServiceProvider.GetRequiredService<MarketDataManager>();
        var tradingManager = scope.ServiceProvider.GetRequiredService<TradingManager>();

        using var _ = Logger.AddScope("Worker", nameof(StockTrader));

        while (!stoppingToken.IsCancellationRequested)
        {
            var timeUntilOpen = await marketDataManager.GetTimeUntilMarketOpen();

            if (timeUntilOpen > TimeSpan.Zero)
            {
                Logger.Information("Market not open, waiting until {nextOpen}", timeUntilOpen);

                await Task.Delay(timeUntilOpen, stoppingToken);
            }
            else
            {
                Logger.Information("Stock Trader executing.");

                await tradingManager.StartTradingAsync();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
