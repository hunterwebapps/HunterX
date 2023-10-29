using Microsoft.Extensions.Hosting;
using Serilog;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupLogging
{
    public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            });

        return hostBuilder;
    }
}
