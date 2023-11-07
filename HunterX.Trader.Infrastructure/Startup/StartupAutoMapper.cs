using HunterX.Trader.Infrastructure.Databases.Entities.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupAutoMapper
{
    public static IHostBuilder AddProfiles(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddAutoMapper(typeof(OrderProfile));
        });
    }
}
