using HunterX.Trader.Workers.Analysis;
using HunterX.Trader.Infrastructure.Startup;
using HunterX.Trader.Infrastructure.Messaging.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .AddConfiguration()
    .AddLogging()
    .AddProfiles()
    .AddMassTransit((appSettings) => new()
    {
        new ConsumerSettings()
        {
            Consumer = typeof(SymbolFeedConsumer),
            ConcurrencyLimit = appSettings.SymbolFeedConcurrency,
        },
    })
    .RegisterDependencies()
    .Build();

host.Run();
