using HunterX.Trader.Infrastructure.Startup;
using HunterX.Trader.Workers.SymbolFeed;

IHost host = Host.CreateDefaultBuilder(args)
    .AddConfiguration()
    .AddLogging()
    .AddMassTransit()
    .AddProfiles()
    .RegisterDependencies()
    .ConfigureServices(services =>
    {
        services.AddHostedService<SymbolFeed>();
    })
    .Build();

host.Run();
