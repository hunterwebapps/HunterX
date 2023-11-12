using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Enums;

namespace HunterX.Trader.Workers.Crypto;

public class CryptoTrader : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public CryptoTrader(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = this.serviceProvider.CreateAsyncScope();

        var marketDataManager = scope.ServiceProvider.GetRequiredService<MarketDataManager>();
        var tradingManager = scope.ServiceProvider.GetRequiredService<TradingManager>();

        using var _ = Logger.AddScope("Worker", nameof(CryptoTrader));

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
                Logger.Information("Crypto Trader executing.");

                await tradingManager.StartTradingAsync(AssetClass.Crypto);

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
