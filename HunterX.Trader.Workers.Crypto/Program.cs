using HunterX.Trader.Workers.Crypto;
using HunterX.Trader.Infrastructure.Startup;
using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.Purchases.Interfaces;
using HunterX.Trader.Infrastructure.Services.MarketData;
using HunterX.Trader.Infrastructure.Services.Brokers.Coinbase;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Infrastructure.DateTimes;
using HunterX.Trader.Infrastructure.Services.Brokers.BackTesting;

IHost host = Host.CreateDefaultBuilder(args)
    .AddConfiguration()
    .AddLogging()
    .AddProfiles()
    .RegisterDependencies((appSettings, services) =>
    {
        services.AddSingleton<IMarketDataService, CryptoMarketDataService>();
        services.AddSingleton<IDataStreamingService, CoinbaseDataStreamingService>();


        if (appSettings.BackTesting.Enabled)
        {
            services.AddSingleton<IBrokerService, BackTestingBrokerService>();
            services.AddSingleton<BackTestingOrderStreamingService>();
            services.AddSingleton<IOrderStreamingService>(x => x.GetRequiredService<BackTestingOrderStreamingService>());
            services.AddSingleton<IDateTimeProvider, DateTimeBackTesting>();
        }
        else
        {
            services.AddSingleton<IBrokerService, CoinbaseBrokerService>();
            services.AddSingleton<IOrderStreamingService, CoinbaseOrderStreamingService>();
            services.AddSingleton<IDateTimeProvider, DateTimeEastern>();
        }
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<CryptoTrader>();
    })
    .Build();

host.Run();
