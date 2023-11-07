using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Infrastructure.Messaging.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupMassTransit
{
    public static IHostBuilder AddMassTransit(this IHostBuilder hostBuilder)
    {
        return AddMassTransit(hostBuilder, (appSettings) => new List<ConsumerSettings>());
    }

    public static IHostBuilder AddMassTransit(this IHostBuilder hostBuilder, Func<AppSettings, List<ConsumerSettings>> setupConsumers)
    {
        hostBuilder
            .ConfigureServices((context, services) =>
            {
                var appSettings = new AppSettings();
                context.Configuration.Bind(appSettings);

                var settings = setupConsumers(appSettings);

                services.AddMassTransit(mtConfig =>
                {
                    mtConfig.SetKebabCaseEndpointNameFormatter();

                    settings.ForEach(c =>
                    {
                        mtConfig.AddConsumer(c.Consumer);
                    });

                    mtConfig.UsingRabbitMq((context, rmqConfig) =>
                    {
                        rmqConfig.Host(appSettings.MassTransit.Host);
                        rmqConfig.PrefetchCount = 1;

                        foreach (var setting in settings)
                        {
                            var endpointName = KebabCaseEndpointNameFormatter.Instance.SanitizeName(setting.Consumer.Name);
                            rmqConfig.ReceiveEndpoint(endpointName, endpointConfig =>
                            {
                                endpointConfig.ConfigureConsumer(context, setting.Consumer);
                                endpointConfig.ConcurrentMessageLimit = setting.ConcurrencyLimit;
                            });
                        }
                    });
                });
            });

        return hostBuilder;
    }
}
