using HunterX.Trader.Infrastructure.Startup;
using HunterX.Trader.Workers.BackTester;

IHost host = Host.CreateDefaultBuilder(args)
    .AddConfiguration()
    .AddLogging()
    .AddEntityFramework()
    .RegisterDependencies()
    .ConfigureServices(services =>
    {
        services.AddHostedService<BackTester>();
    })
    .Build();

host.Run();
