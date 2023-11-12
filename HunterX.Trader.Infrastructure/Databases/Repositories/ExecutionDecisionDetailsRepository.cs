using AutoMapper;
using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.Common.Interfaces.Repositories;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace HunterX.Trader.Infrastructure.Databases.Repositories;

public class ExecutionDecisionDetailsRepository : IExecutionDecisionDetailsRepository
{
    private readonly TradingMongoDbContext mongoContext;
    private readonly IMapper mapper;

    public ExecutionDecisionDetailsRepository(TradingMongoDbContext mongoContext, IMapper mapper)
    {
        this.mongoContext = mongoContext;
        this.mapper = mapper;
    }

    public async Task<ExecutionDecisionDetails> GetExecutionDecisionDetails(Guid id)
    {
        var document = await this.mongoContext.ExecutionDecisionDetails.Find(x => x._id == id.ToString()).SingleAsync();

        var domainEntity = this.mapper.Map<ExecutionDecisionDetails>(document);

        return domainEntity;
    }

    public async Task<PaginatedList<ExecutionDecisionBasics>> GetExecutionDecisionBasicsAsync(int pageNumber, int pageSize)
    {
        var projection = Builders<Documents.ExecutionDecisionDetails>.Projection
            .Include(x => x._id)
            .Include(x => x.ExecutionDecision)
            .Include(x => x.StrategyName)
            .Include(x => x.CreatedAt);

        var documents = await this.mongoContext.ExecutionDecisionDetails
            .Find(FilterDefinition<Documents.ExecutionDecisionDetails>.Empty)
            .Project<BsonDocument>(projection)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        var basics = documents
            .Select(x => new ExecutionDecisionBasics()
            {
                Id = Guid.Parse(x["_id"].AsString),
                ExecutionDecision = BsonSerializer.Deserialize<ExecutionDecision>(x["ExecutionDecision"].AsBsonDocument),
                StrategyName = x["StrategyName"].AsString,
                CreatedAt = x["CreatedAt"].ToUniversalTime(),
            })
            .ToList();

        var totalDocuments = await this.mongoContext.ExecutionDecisionDetails.CountDocumentsAsync(FilterDefinition<Documents.ExecutionDecisionDetails>.Empty);

        return new PaginatedList<ExecutionDecisionBasics>()
        {
            Items = basics,
            Count = totalDocuments,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }

    public async Task SaveDecisionDetailsAsync(ExecutionDecisionDetails decisionDetails)
    {
        var documents = new Documents.ExecutionDecisionDetails()
        {
            _id = decisionDetails.Id.ToString(),
            Symbol = decisionDetails.ExecutionDecision.Symbol,
            ExecutionDecision = decisionDetails.ExecutionDecision,
            StockBasics = decisionDetails.Stock,
            StrategyName = decisionDetails.StrategyName,
            Bars = decisionDetails.Bars,
            CreatedAt = DateTime.UtcNow,
        };

        await this.mongoContext.ExecutionDecisionDetails.InsertOneAsync(documents);
    }
}
