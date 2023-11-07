using HunterX.Trader.Infrastructure.Databases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupEntityFramework
{
    public static IHostBuilder AddEntityFramework(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<TradingSqlDbContext>();
            });

        return hostBuilder;
    }
}
