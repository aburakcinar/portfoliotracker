//using FinanceData.Business.Api;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using PortfolioTracker.Data.Models;

//namespace PortfolioTracker.WebApp.Business.Requests.Holdings;

//public sealed class HoldingAssetSummaryModel
//{
//    public Guid Id { get; init; }

//    public string Symbol { get; init; } = string.Empty;

//    public string Name { get; init; } = string.Empty;

//    public decimal Price { get; init; }
    
//    public string AssetCurrency { get; init; } = string.Empty;

//    public decimal PriceInPortfolioCurrency { get; init; }

//    public string PortfolioCurrency { get; init; } = string.Empty;
    
//    public decimal Quantity { get; init; }

//    public decimal TotalProfitLoss { get; init; }

//    public decimal RealizedProfitLoss { get; init; }

//    public decimal UnrealizedProfitLoss { get; init; }

//    public decimal TotalFees { get; init; }

//    public decimal TotalTaxes { get; init; }
//}

//public class GetHoldingAssetSummaryRequest : IRequest<HoldingAssetSummaryModel>
//{
//    public Guid PortfolioId { get; init; }

//    public Guid AssetId { get; init; }
//}

//public sealed class GetHoldingAssetSummaryRequestHandler : IRequestHandler<GetHoldingAssetSummaryRequest, HoldingAssetSummaryModel>
//{
//    private readonly IPortfolioContext m_context;
//    private readonly ICurrencyRateService m_currencyRateService;

//    public GetHoldingAssetSummaryRequestHandler(
//        IPortfolioContext context,
//        ICurrencyRateService currencyRateService)
//    {
//        m_context = context;
//        m_currencyRateService = currencyRateService;
//    }

//    public async Task<HoldingAssetSummaryModel> Handle(GetHoldingAssetSummaryRequest request, CancellationToken cancellationToken)
//    {
//        var holdings = await m_context.Holdings
//            .Include(x => x.Asset)
//            .Include(x => x.Portfolio)
//            .ThenInclude(x => x.BankAccount)
//            .Include(x => x.BankAccountTransactionGroup)
//            .ThenInclude(x => x.Transactions)
//            .Where(x => x.PortfolioId == request.PortfolioId && x.AssetId == request.AssetId)
//            .ToListAsync(cancellationToken);

//        if (!holdings.Any())
//        {
//            return new HoldingAssetSummaryModel();
//        }

//        var asset = holdings.First().Asset;

//        var assetCurrency = holdings.First().Asset.CurrencyCode;
//        var portfolioCurrency = holdings.First().Portfolio.BankAccount.CurrencyCode;

//        var priceInPortfolioCurrency = await m_currencyRateService.ConvertAsync(asset.Price, new CurrencyRateQueryModel
//        {
//            Base = assetCurrency,
//            Target = portfolioCurrency,
//            Date = DateOnly.FromDateTime(DateTime.Now.Date.ToUniversalTime())
//        });

//        decimal totalQuantity = 0;
//        decimal totalCost = 0;
//        decimal realizedProfitLoss = 0;
//        decimal totalFees = 0;
//        decimal totalTaxes = 0;

//        foreach (var holding in holdings)
//        {
//            foreach (var transaction in holding.BankAccountTransactionGroup.Transactions)
//            {
//                switch (transaction.ActionTypeCode)
//                {
//                    case nameof(TransactionActionTypes.BUY_ASSET):
//                        totalQuantity += transaction.Quantity;
//                        totalCost += transaction.Quantity * transaction.Price;
//                        break;

//                    case nameof(TransactionActionTypes.SELL_ASSET):
//                        var sellAmount = transaction.Quantity * transaction.Price;
//                        var avgCostForSoldShares = (totalCost / totalQuantity) * transaction.Quantity;
//                        realizedProfitLoss += sellAmount - avgCostForSoldShares;

//                        totalQuantity -= transaction.Quantity;
//                        totalCost -= avgCostForSoldShares;
//                        break;

//                    case nameof(TransactionActionTypes.BUY_ASSET_FEE):
//                    case nameof(TransactionActionTypes.SELL_ASSET_FEE):
//                        totalFees += transaction.Price;
//                        break;

//                    case nameof(TransactionActionTypes.BUY_ASSET_TAX):
//                    case nameof(TransactionActionTypes.SELL_ASSET_TAX):
//                        totalTaxes += transaction.Price;
//                        break;
//                }
//            }
//        }

//        var currentMarketValue = totalQuantity * asset.Price;
//        var unrealizedProfitLoss = currentMarketValue - totalCost;
//        var totalProfitLoss = realizedProfitLoss + unrealizedProfitLoss;

//        return new HoldingAssetSummaryModel
//        {
//            Id = request.AssetId,
//            Symbol = asset.TickerSymbol,
//            Name = asset.Name,
//            Price = asset.Price,
//            AssetCurrency = assetCurrency,
//            PriceInPortfolioCurrency = priceInPortfolioCurrency, // Note: Currency conversion would be needed here if currencies differ
//            PortfolioCurrency = portfolioCurrency,
//            Quantity = totalQuantity,
//            TotalProfitLoss = totalProfitLoss,
//            RealizedProfitLoss = realizedProfitLoss,
//            UnrealizedProfitLoss = unrealizedProfitLoss,
//            TotalFees = totalFees,
//            TotalTaxes = totalTaxes
//        };
//    }
//}