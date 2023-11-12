using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.Purchases.Interfaces;
using HunterX.Trader.Infrastructure.Services.Brokers.Alpaca;
using HunterX.Trader.Infrastructure.Services.Brokers.BackTesting;
using HunterX.Trader.Infrastructure.Services.MarketData;
using HunterX.Trader.Infrastructure.Startup;
using HunterX.Trader.Workers.Stocks;

IHost host = Host.CreateDefaultBuilder(args)
    .AddConfiguration()
    .AddLogging()
    .AddProfiles()
    .RegisterDependencies((appSettings, services) =>
    {
        services.AddSingleton<IMarketDataService, StockMarketDataService>();
        services.AddSingleton<IDataStreamingService, AlpacaDataStreamingService>();

        if (appSettings.BackTesting.Enabled)
        {
            services.AddSingleton<IBrokerService, BackTestingBrokerService>();
            services.AddSingleton<BackTestingOrderStreamingService>();
            services.AddSingleton<IOrderStreamingService>(x => x.GetRequiredService<BackTestingOrderStreamingService>());
        }
        else
        {
            services.AddSingleton<IBrokerService, AlpacaBrokerService>();
            services.AddSingleton<IOrderStreamingService, AlpacaOrderStreamingService>();
        }
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<StockTrader>();
        services.AddHostedService<OrderMonitor>();
    })
    .Build();

host.Run();
