using HunterX.Trader.Workers.Executor;
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
            Consumer = typeof(ExecutionDecisionConsumer),
        },
        //new ConsumerSettings()
        //{
        //    Consumer = typeof(SellingMonitorConsumer),
        //    ConcurrencyLimit = 100,
        //},
    })
    .RegisterDependencies()
    .ConfigureServices(services =>
    {
        //services.AddHostedService<SellerFeed>();
    })
    .Build();

host.Run();
