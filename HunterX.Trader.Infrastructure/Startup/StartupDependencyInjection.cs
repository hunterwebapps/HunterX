using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Domain.Common.DateTime;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.Purchase.Interfaces;
using HunterX.Trader.Infrastructure.Databases.Repositories;
using HunterX.Trader.Infrastructure.Messaging;
using HunterX.Trader.Infrastructure.Services.Brokers.Alpaca;
using HunterX.Trader.Infrastructure.Services.Brokers.BackTesting;
using HunterX.Trader.Infrastructure.Services.MarketData;
using HunterX.Trader.Infrastructure.Services.MarketData.Alpaca;
using HunterX.Trader.Infrastructure.Services.MarketData.FinancialModelingPrep;
using HunterX.Trader.Infrastructure.Services.MarketData.Polygon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupDependencyInjection
{
    public static IHostBuilder RegisterDependencies(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .AddEntityFramework()
            .AddMongo()
            .ConfigureServices((hostContext, services) =>
            {
                var appSettings = new AppSettings();
                hostContext.Configuration.Bind(appSettings);

                // Messaging Handlers
                services.AddScoped<ISymbolFeedService, StockFeedPublisher>();
                services.AddScoped<IBuySignalService, BuySignalPublisher>();

                // Market Data Services
                services.AddSingleton<IMarketDataService, MarketDataService>();
                services.AddSingleton<FMPDataService>();
                services.AddSingleton<PolygonDataService>();
                services.AddSingleton<AlpacaDataService>();

                // Broker Service
                if (appSettings.BackTesting.Enabled)
                {
                    services.AddSingleton<BackTestingOrderStreamingService>();
                    services.AddSingleton<IBrokerService, BackTestingBrokerService>();
                    services.AddSingleton<IOrderStreamingService>(x => x.GetRequiredService<BackTestingOrderStreamingService>());
                }
                else
                {
                    services.AddSingleton<IBrokerService, AlpacaBrokerService>();
                    services.AddSingleton<IOrderStreamingService, AlpacaOrderStreamingService>();
                }

                // Managers
                services.AddScoped<AnalysisManager>();
                services.AddScoped<MarketDataManager>();
                services.AddScoped<SymbolFeedManager>();
                services.AddScoped<OrderManager>();
                services.AddScoped<ExecutionDecisionManager>();

                // Repositories
                services.AddScoped<IOrderRepository, OrderRepository>();
                services.AddScoped<IExecutionDecisionDetailsRepository, ExecutionDecisionDetailsRepository>();

                // Date Time
                services.AddSingleton<IDateTimeProvider>((services) =>
                {
                    return appSettings.BackTesting.Enabled
                        ? new DateTimeEastern(appSettings.BackTesting.ESTNow)
                        : new DateTimeEastern();
                });

                services.AddHttpClient();
            });

        return hostBuilder;
    }
}
