using HunterX.Trader.Application.Managers;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.Common.Interfaces.Repositories;
using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading.Purchases.Interfaces;
using HunterX.Trader.Infrastructure.Databases.Repositories;
using HunterX.Trader.Infrastructure.DateTimes;
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
    public static IHostBuilder RegisterDependencies(this IHostBuilder hostBuilder, Action<AppSettings, IServiceCollection> configureServices)
    {
        hostBuilder
            .AddEntityFramework()
            .AddMongo()
            .ConfigureServices((hostContext, services) =>
            {
                var appSettings = new AppSettings();
                hostContext.Configuration.Bind(appSettings);

                // Market Data Services
                services.AddSingleton<AlpacaDataService>();
                services.AddSingleton<FMPDataService>();
                services.AddSingleton<PolygonDataService>();

                // Managers
                services.AddScoped<TradingManager>();
                services.AddScoped<AnalysisManager>();
                services.AddScoped<MarketDataManager>();
                services.AddScoped<OrderManager>();
                services.AddScoped<ExecutionDecisionManager>();

                // Repositories
                services.AddScoped<IOrderRepository, OrderRepository>();
                services.AddScoped<IExecutionDecisionDetailsRepository, ExecutionDecisionDetailsRepository>();

                services.AddHttpClient();

                configureServices(appSettings, services);
            });

        return hostBuilder;
    }
}
