using HunterX.Trader.Application.Managers;
using HunterX.Trader.Application.Services.Interfaces;
using HunterX.Trader.Infrastructure.Services.MarketData.Alpaca;
using HunterX.Trader.Infrastructure.Services.ReferenceData;
using HunterX.Trader.Infrastructure.Services.ReferenceData.Polygon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupDependencyInjection
{
    public static IHostBuilder RegisterDependencies(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IReferenceDataService, PolygonReferenceDataService>();
                services.AddSingleton<IMarketDataService, AlpacaMarketDataService>();

                services.AddTransient<AnalysisManager>();

                services.AddTransient<ReferenceDataRepository>();

                services.AddHttpClient();
            });

        return hostBuilder;
    }
}
