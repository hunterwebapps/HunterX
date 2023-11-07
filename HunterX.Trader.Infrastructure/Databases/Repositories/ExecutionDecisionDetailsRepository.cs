using AutoMapper;
using HunterX.Trader.Application.Interfaces;
using HunterX.Trader.Domain.Common;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData;
using HunterX.Trader.Domain.StrategySelection.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.StrategySelection.Strategies.Enum;
using MongoDB.Bson;
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
        try
        {
            var document = await this.mongoContext.ExecutionDecisionDetails.Find(x => x._id == id.ToString()).SingleAsync();

            var domainEntity = this.mapper.Map<ExecutionDecisionDetails>(document);

            return domainEntity;
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
                ExecutionDecision = new ExecutionDecision()
                {
                    Action = (TradeAction)x["ExecutionDecision"]["Action"].AsInt32,
                    Symbol = x["ExecutionDecision"]["Symbol"].AsString,
                    Price = decimal.Parse(x["ExecutionDecision"]["Price"].AsString),
                    Stop = x["ExecutionDecision"]["StopLoss"].AsNullableDecimal,
                    SignalTime = x["ExecutionDecision"]["SignalTime"].ToNullableUniversalTime(),
                },
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

    public async Task SaveDecisionDetailsAsync(IReadOnlyList<ExecutionDecisionDetails> decisionDetails)
    {
        var documents = decisionDetails.Select(x => new Documents.ExecutionDecisionDetails()
        {
            _id = x.Id.ToString(),
            Symbol = x.ExecutionDecision.Symbol,
            ExecutionDecision = x.ExecutionDecision,
            StockBasics = x.StockBasics,
            StrategyName = x.StrategyName,
            Bars = x.Bars,
            Indicators = new TechnicalIndicators()
            {
                ShortEMA = x.Indicators.ShortEMA,
                LongEMA = x.Indicators.LongEMA,
                RollingPivots = x.Indicators.RollingPivots,
                Pivots = x.Indicators.Pivots,
                PivotPoints = x.Indicators.PivotPoints,
                MovingAverageConvergenceDivergence = x.Indicators.MovingAverageConvergenceDivergence,
                VolumeWeightedAvgPrice = x.Indicators.VolumeWeightedAvgPrice,
                RelativeStrengthIndex = x.Indicators.RelativeStrengthIndex,
                AverageTrueRange = x.Indicators.AverageTrueRange,
            },
            CreatedAt = DateTime.UtcNow,
        });

        await this.mongoContext.ExecutionDecisionDetails.InsertManyAsync(documents);
    }
}
