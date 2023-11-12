using HunterX.Trader.Domain.Trading.Purchases.Interfaces;
using HunterX.Trader.Domain.Trading.Purchases.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.ValueObjects;
using System.Text.Json;
using System;

namespace HunterX.Trader.Infrastructure.Services.Brokers.Coinbase;

public class CoinbaseBrokerService : IBrokerService
{
    private const string apiDomain = "";
    private readonly IHttpClientFactory httpClientFactory;

    public CoinbaseBrokerService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public Task<Order> ExecuteOrderAsync(ExecutionDecision executionDecision, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetBuyingPowerAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOpenOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Position>> GetOpenPositionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Position> GetPositionAsync(string symbol)
    {
        throw new NotImplementedException();
    }

    private async Task<T> GetRequestAsync<T>(string relativePath, Dictionary<string, string> queryParams)
    {
        var client = this.httpClientFactory.CreateClient();

        var queryString = string.Join("&", queryParams.Select(t => $"{t.Key}={t.Value}"));

        var url = $"{apiDomain}{relativePath}?{queryString}&apikey={this.apiKey}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var cancellationToken = new CancellationToken(false);
        var response = await this.apiThrottler.ThrottledRequestAsync(() => client.SendAsync(request, cancellationToken), cancellationToken);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(json, jsonSerializerOptions) ?? throw new JsonException("Received null from json deserialization.");

        return result;
    }
}
