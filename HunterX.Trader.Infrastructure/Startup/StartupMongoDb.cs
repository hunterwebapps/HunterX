using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Infrastructure.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace HunterX.Trader.Infrastructure.Startup;

public static class StartupMongoDb
{
    public static IHostBuilder AddMongo(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .ConfigureServices((context, services) =>
            {
                var appSettings = new AppSettings();
                context.Configuration.Bind(appSettings);

                var objectSerializer = new ObjectSerializer(type => true);
                BsonSerializer.RegisterSerializer(objectSerializer);

                var client = new MongoClient(appSettings.ConnectionStrings.MongoDb);

                var dbContext = new TradingMongoDbContext(client);

                services.AddSingleton(dbContext);
            });

        return hostBuilder;
    }
}
