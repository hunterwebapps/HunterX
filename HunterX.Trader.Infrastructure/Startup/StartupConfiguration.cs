using Azure.Identity;
using HunterX.Trader.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupConfiguration
{
    public static IHostBuilder AddConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true);

                var envName = hostingContext.HostingEnvironment.EnvironmentName;

                config.AddJsonFile(
                    $"appsettings.{envName}.json",
                    optional: false,
                    reloadOnChange: true);

                var prebuildConfig = config.Build();
                var azureUri = new Uri(prebuildConfig["AzureKeyVaultUri"]!);

                config.AddAzureKeyVault(azureUri, new DefaultAzureCredential());
            })
            .ConfigureServices((hostingContext, services) =>
            {
                var appSettings = new AppSettings();

                hostingContext.Configuration.Bind(appSettings);

                services.AddSingleton(appSettings);
            });

        return hostBuilder;
    }
}
