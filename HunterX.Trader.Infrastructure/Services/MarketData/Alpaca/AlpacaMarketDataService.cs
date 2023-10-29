using Alpaca.Markets;
using HunterX.Trader.Application.Services.Interfaces;
using HunterX.Trader.Common.Configuration;

namespace HunterX.Trader.Infrastructure.Services.MarketData.Alpaca;

public class AlpacaMarketDataService : IMarketDataService
{
    private readonly IAlpacaDataClient dataClient;
    private readonly ApiThrottler apiThrottler;

    public AlpacaMarketDataService(AppSettings appSettings)
    {
        var secretKey = new SecretKey(appSettings.Alpaca.Key, appSettings.Alpaca.Secret);

        dataClient = appSettings.Live
            ? Environments.Live.GetAlpacaDataClient(secretKey)
            : Environments.Paper.GetAlpacaDataClient(secretKey);

        apiThrottler = new ApiThrottler(200, TimeSpan.FromMinutes(1));
    }
}
