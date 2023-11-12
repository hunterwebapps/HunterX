using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace HunterX.Trader.Infrastructure.Databases.Documents;

public class ExecutionDecisionDetails
{
    [BsonIgnore]
    public const string CollectionName = "execution-decision-details";

#pragma warning disable IDE1006 // Naming Styles
    [BsonId]
    public required string _id { get; init; }
#pragma warning restore IDE1006 // Naming Styles
    public required string Symbol { get; init; }
    public required IReadOnlyList<Bar> Bars { get; set; }
    public required ExecutionDecision ExecutionDecision { get; init; }
    public required Asset StockBasics { get; init; }
    public required string StrategyName { get; init; }
    public required DateTime CreatedAt { get; set; }
}
