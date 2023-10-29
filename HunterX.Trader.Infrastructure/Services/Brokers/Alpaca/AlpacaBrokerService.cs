using Alpaca.Markets;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Domain.Purchase.Interfaces;

namespace HunterX.Trader.Infrastructure.Services.Brokers.Alpaca;

public class AlpacaBrokerService : IBrokerService
{
    private readonly IAlpacaTradingClient tradingClient;
    private readonly ApiThrottler apiThrottler;

    public AlpacaBrokerService(AppSettings appSettings)
    {
        var secretKey = new SecretKey(appSettings.Alpaca.Key, appSettings.Alpaca.Secret);

        tradingClient = appSettings.Live
            ? Environments.Live.GetAlpacaTradingClient(secretKey)
            : Environments.Paper.GetAlpacaTradingClient(secretKey);

        apiThrottler = new ApiThrottler(200, TimeSpan.FromMinutes(1));
    }
}
