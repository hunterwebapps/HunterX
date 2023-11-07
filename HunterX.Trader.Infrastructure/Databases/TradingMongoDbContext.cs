using HunterX.Trader.Infrastructure.Databases.Documents;
using MongoDB.Driver;

namespace HunterX.Trader.Infrastructure.Databases;

public class TradingMongoDbContext
{
    private readonly IMongoClient client;

    public TradingMongoDbContext(IMongoClient client)
    {
        this.client = client;

        CollectionConfiguration.Configure(this.Database);
    }

    public IMongoDatabase Database => this.client.GetDatabase("Trading");

    // Collections
    public IMongoCollection<ExecutionDecisionDetails> ExecutionDecisionDetails => this.Database.GetCollection<ExecutionDecisionDetails>(Documents.ExecutionDecisionDetails.CollectionName);
}
