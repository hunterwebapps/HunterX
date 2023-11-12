using HunterX.Trader.Common.Logging;
using HunterX.Trader.Domain.Common.Enums;
using HunterX.Trader.Domain.Common.Interfaces;
using HunterX.Trader.Domain.Common.Interfaces.Repositories;
using HunterX.Trader.Domain.Common.Interfaces.Services;
using HunterX.Trader.Domain.Trading;
using HunterX.Trader.Domain.Trading.Purchases.Interfaces;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.DecisionData.ValueObjects;
using HunterX.Trader.Domain.Trading.StrategySelections.Strategies.Enum;
using HunterX.Trader.Domain.Trading.StrategySelections.ValueObjects;

namespace HunterX.Trader.Application.Managers;

public class TradingManager
{
    private readonly IBrokerService brokerService;
    private readonly IMarketDataService marketDataService;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IOrderRepository orderRepository;
    private readonly IExecutionDecisionDetailsRepository executionDecisionDetailsRepository;

    private readonly IDataStreamingService dataStreamingService;
    private bool isStreamingConnected = false;

    private readonly TradingRoot trading;

    public TradingManager(
        IDataStreamingService dataStreamingService,
        IBrokerService brokerService,
        IMarketDataService marketDataService,
        IDateTimeProvider dateTimeProvider,
        IOrderRepository orderRepository,
        IExecutionDecisionDetailsRepository executionDecisionDetailsRepository)
    {
        this.dataStreamingService = dataStreamingService;
        this.brokerService = brokerService;
        this.marketDataService = marketDataService;
        this.dateTimeProvider = dateTimeProvider;
        this.orderRepository = orderRepository;
        this.executionDecisionDetailsRepository = executionDecisionDetailsRepository;

        this.trading = new TradingRoot();

        this.dataStreamingService.Connected += (sender, args) =>
        {
            this.isStreamingConnected = true;
        };

        this.dataStreamingService.Disconnected += (sender, args) =>
        {
            this.isStreamingConnected = false;
        };

        this.dataStreamingService.Unsubscribed += (sender, stock) =>
        {
            this.trading?.StopTrading(stock.Symbol);
        };
    }

    public async Task StopTradingAsync()
    {
        if (this.isStreamingConnected)
        {
            await this.dataStreamingService.DisconnectAsync();
        }
    }

    public async Task StartTradingAsync()
    {
        if (!this.isStreamingConnected)
        {
            await this.dataStreamingService.ConnectAsync();
        }

        var stocks = await GetTradeableStocksAsync();

        Parallel.ForEach(stocks, async stock =>
        {
            if (this.trading.IsTrading(stock.Symbol))
            {
                return;
            }

            var bars = await this.marketDataService.GetChartDataAsync(new ChartDataParams()
            {
                Symbol = stock.Symbol,
                TimeFrame = "1min",
                StartDate = this.dateTimeProvider.Now.Date,
                EndDate = this.dateTimeProvider.Now.DateTime,
            });

            this.trading.StartTrading(stock, bars);

            await this.dataStreamingService.RegisterSubscriptionHandlerAsync(HandleBarReceived, stock);
        });
    }

    private async Task HandleBarReceived(Bar bar)
    {
        var details = this.trading.EvaluateBarDecision(bar);

        if (!details.ExecutionDecision.IsOrderDecision)
        {
            return;
        }

        Logger.Information("Found Order Decision {@decision}", details.ExecutionDecision);

        int quantity;

        if (details.ExecutionDecision.Action == TradeAction.Buy)
        {
            var buyingPower = await this.brokerService.GetBuyingPowerAsync();

            quantity = GetRiskAdjustedQuantity(
                buyingPower,
                details.ExecutionDecision.Price!.Value,
                details.ExecutionDecision.StopLoss!.Value);
        }
        else if (details.ExecutionDecision.Action == TradeAction.Sell)
        {
            var position = await this.brokerService.GetPositionAsync(details.ExecutionDecision.Symbol);

            quantity = position.Quantity;
        }
        else
        {
            throw new Exception($"Unknown TradeAction {details.ExecutionDecision.Action}");
        }

        var order = await this.brokerService.ExecuteOrderAsync(details.ExecutionDecision, quantity);

        await this.orderRepository.SaveOrderAsync(order);

        await this.executionDecisionDetailsRepository.SaveDecisionDetailsAsync(details);
    }

    private async Task<IEnumerable<Asset>> GetTradeableStocksAsync()
    {
        var qualifiedStocks = await this.marketDataService.GetAssetsAsync();

        var openPositions = await this.brokerService.GetOpenPositionsAsync();

        var openSymbols = openPositions
            .Select(x => x.Symbol)
            .Where(x => !this.trading.IsTrading(x));

        var openPositionStocks = await this.marketDataService.GetAssetsAsync(openSymbols);

        var tradeableStocks = qualifiedStocks.Union(openPositionStocks);

        return tradeableStocks;
    }

    private static int GetRiskAdjustedQuantity(decimal buyingPower, decimal price, decimal stopLoss)
    {
        var maxRiskPrice = buyingPower * 0.0025m;

        var riskPerShare = price - stopLoss;

        var shares = (int)Math.Floor(maxRiskPrice / riskPerShare);

        if (shares * price > buyingPower)
        {
            shares = (int)Math.Floor(buyingPower / price);
        }

        return shares;
    }
}
