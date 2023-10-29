using HunterX.Trader.Workers.Executor;
using HunterX.Trader.Infrastructure.Startup;

IHost host = Host.CreateDefaultBuilder(args)
    .AddConfiguration()
    .AddLogging()
    .AddEntityFramework()
    .RegisterDependencies()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Buyer>();
    })
    .Build();

host.Run();
