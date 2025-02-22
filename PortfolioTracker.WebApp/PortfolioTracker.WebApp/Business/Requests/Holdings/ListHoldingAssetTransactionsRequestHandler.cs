using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.Holdings;

public sealed class HoldingAssetTransactionModel
{
    public Guid Id { get; init; }
    
    public DateOnly ExecuteDate { get; init; }
    
    public string Description { get; init; } = string.Empty;
    
    public decimal Quantity { get; init; }
    
    public decimal Price { get; init; }
    
    public decimal Total { get; init; }
    
    public decimal Expenses { get; init; }

    public decimal Taxes { get; init; }
    
    public string CurrencyCode { get; init; } = string.Empty;

    public string CurrencySymbol { get; init; } = string.Empty;
}

public sealed class ListHoldingAssetTransactionsRequest : IRequest<IEnumerable<HoldingAssetTransactionModel>>
{
    public Guid PortfolioId { get; init; }
    
    public Guid AssetId { get; init; }
}

public sealed class ListHoldingAssetTransactionsRequestHandler : IRequestHandler<ListHoldingAssetTransactionsRequest, IEnumerable<HoldingAssetTransactionModel>>
{
    private readonly PortfolioContext m_context;

    public ListHoldingAssetTransactionsRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<HoldingAssetTransactionModel>> Handle(ListHoldingAssetTransactionsRequest request, CancellationToken cancellationToken)
    {
        var portfolio = await m_context
            .Portfolios
            .Include(x => x.BankAccount)
            .ThenInclude(x => x.Currency)
            .FirstOrDefaultAsync(x => x.Id == request.PortfolioId,cancellationToken);

        if (portfolio == null)
        {
            return Enumerable.Empty<HoldingAssetTransactionModel>();
        }
        
        var lst = await m_context
            .Holdings
            .Include(x => x.BankAccountTransactionGroup)
            .ThenInclude(x => x.Transactions)
            .Where(x => x.PortfolioId == request.PortfolioId && x.AssetId == request.AssetId)
            .ToListAsync(cancellationToken);

        var result = lst.Select(x =>
        {
            var mainTransaction = x
                .BankAccountTransactionGroup
                .Transactions
                .FirstOrDefault(x =>
                    x.ActionTypeCode == TransactionActionTypes.BUY_ASSET ||
                    x.ActionTypeCode == TransactionActionTypes.SELL_ASSET);
                
            
            return new HoldingAssetTransactionModel
            {
                Id = x.Id,
                ExecuteDate = x.ExecuteDate,
                Description = x.BankAccountTransactionGroup.Description,
                CurrencyCode = portfolio.BankAccount.CurrencyCode,
                CurrencySymbol = portfolio.BankAccount.Currency.Symbol,
                Quantity = mainTransaction?.Quantity ?? 0,
                Price = mainTransaction?.Price ?? 0,
                Total = mainTransaction != null ? mainTransaction.Quantity * mainTransaction.Price * (mainTransaction.InOut == InOut.Outgoing ? -1 : 1) : 0,
                Expenses = x.BankAccountTransactionGroup
                    .Transactions
                    .FirstOrDefault(x => 
                        x.ActionTypeCode == TransactionActionTypes.BUY_ASSET_FEE 
                        || x.ActionTypeCode == TransactionActionTypes.SELL_ASSET_FEE)?.Price ?? 0,
                Taxes = x.BankAccountTransactionGroup
                    .Transactions
                    .FirstOrDefault(x => 
                        x.ActionTypeCode == TransactionActionTypes.BUY_ASSET_TAX 
                        || x.ActionTypeCode == TransactionActionTypes.SELL_ASSET_TAX)?.Price ?? 0,
            };
        });
        
        return result;
    }
}