using MongoDB.Driver;

namespace HunterX.Trader.Infrastructure.Databases.Documents;

public static class CollectionConfiguration
{
    public static void Configure(IMongoDatabase database)
    {
        ConfigureExecutionDecisionDetails(database);
    }

    private static void ConfigureExecutionDecisionDetails(IMongoDatabase database)
    {
        var collection = database.GetCollection<ExecutionDecisionDetails>(ExecutionDecisionDetails.CollectionName);

        var symbolIndex = Builders<ExecutionDecisionDetails>.IndexKeys.Ascending(x => x.Symbol);
        var createdAtIndex = Builders<ExecutionDecisionDetails>.IndexKeys.Descending(x => x.CreatedAt);

        var symbolIndexModel = new CreateIndexModel<ExecutionDecisionDetails>(symbolIndex);
        var createdAtIndexModel = new CreateIndexModel<ExecutionDecisionDetails>(createdAtIndex);

        collection.Indexes.CreateMany(new[]
        {
            symbolIndexModel,
            createdAtIndexModel,
        });
    }
}
