using HunterX.Trader.Workers.Analysis;
using HunterX.Trader.Infrastructure.Startup;

IHost host = Host.CreateDefaultBuilder(args)
    .AddConfiguration()
    .AddLogging()
    .AddEntityFramework()
    .RegisterDependencies()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Analysis>();
    })
    .Build();

host.Run();
