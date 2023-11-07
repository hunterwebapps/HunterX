using Alpaca.Markets;
using HunterX.Trader.Common.Configuration;
using HunterX.Trader.Domain.Purchase.ValueObjects;

namespace HunterX.Trader.Infrastructure.Services.MarketData.Alpaca;

public class AlpacaDataService
{
    private readonly IAlpacaDataClient dataClient;

    public AlpacaDataService(AppSettings appSettings)
    {
        var secretKey = new SecretKey(appSettings.Alpaca.Key, appSettings.Alpaca.Secret);

        this.dataClient = appSettings.Live
            ? Environments.Live.GetAlpacaDataClient(secretKey)
            : Environments.Paper.GetAlpacaDataClient(secretKey);
    }

    public async Task<IReadOnlyList<OrderPrice>> GetOrderPriceAsync(string symbol, DateTime from, DateTime to)
    {
        var response = await this.dataClient.GetHistoricalQuotesAsync(new HistoricalQuotesRequest(symbol, from, to));

        var quotes = response.Items[symbol];

        return quotes
            .Select(quote => new OrderPrice()
            {
                AskPrice = quote.AskPrice,
                AskSize = quote.AskSize,
                BidPrice = quote.BidPrice,
                BidSize = quote.BidSize,
                Timestamp = quote.TimestampUtc,
            })
            .ToList();
    }
}
