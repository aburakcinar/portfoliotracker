using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.PortfolioV2Entity;

public sealed class PortfolioTotalPositionResultModel
{
    public Guid PortfolioId { get; init; }

    public decimal TotalPosition { get; init; }
    
    public decimal TotalCost { get; init; }
    
    public decimal TotalExpenses { get; init; }
    
    public required string CurrencyCode { get; init; }

    public required string CurrencySymbol { get; init; }
}

public sealed class GetPortfolioTotalPositionRequest : IRequest<PortfolioTotalPositionResultModel?>
{
    public Guid PortfolioId { get; init; }
}

public sealed class GetPortfolioTotalPositionRequestHandler : 
    IRequestHandler<GetPortfolioTotalPositionRequest, PortfolioTotalPositionResultModel?>
{
    private readonly PortfolioContext m_context;

    public GetPortfolioTotalPositionRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<PortfolioTotalPositionResultModel?> Handle(GetPortfolioTotalPositionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var portfolio = await m_context
                    .Portfolios
                    .Include(x => x.BankAccount)
                    .ThenInclude(x => x.Currency)
                    .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

            if (portfolio == null)
            {
                return null;
            }

            var holdings = await m_context
                .Holdings
                .Include(x => x.BankAccountTransactionGroup)
                .ThenInclude(x => x.Transactions)
                .Include(x => x.Asset)
                .Where(x => x.PortfolioId == request.PortfolioId)
                .ToListAsync(cancellationToken);

            decimal runningTotal = 0M;
            decimal runningCost = 0M;
            decimal runningExpense = 0M;

            var totalTransactionActions = new List<string>
            {
                nameof(TransactionActionTypes.BUY_ASSET)
            };
            
            var costTransactionActions = new List<string>
            {
                nameof(TransactionActionTypes.BUY_ASSET),
                nameof(TransactionActionTypes.SELL_ASSET)
            }; 
            
            var expenseTransactionActions = new List<string>
            {
                nameof(TransactionActionTypes.BUY_ASSET_FEE),
                nameof(TransactionActionTypes.SELL_ASSET_FEE),
                nameof(TransactionActionTypes.DEPOSIT_FEE),
                nameof(TransactionActionTypes.DEPOSIT_TAX),
                nameof(TransactionActionTypes.WITHDRAW_FEE),
                nameof(TransactionActionTypes.WITHDRAW_TAX),
                nameof(TransactionActionTypes.ACCOUNT_FEE),
            };

            foreach (var holding in holdings)
            {
                foreach (var transaction in holding.BankAccountTransactionGroup.Transactions)
                {
                    if (transaction is { ActionTypeCode: nameof(TransactionActionTypes.BUY_ASSET) })
                    {
                        runningTotal += transaction.Quantity * holding.Asset.Price;
                        runningCost += transaction.Quantity * transaction.Price;
                        
                    }   
                    else if (transaction is { ActionTypeCode: nameof(TransactionActionTypes.SELL_ASSET) })
                    {
                        runningTotal -= transaction.Quantity * holding.Asset.Price;
                        runningCost -= transaction.Quantity * transaction.Price;
                    }
                    else if (expenseTransactionActions.Contains(transaction.ActionTypeCode))
                    {
                        runningExpense += transaction.Quantity * transaction.Price;
                    }
                }
            }

            return new PortfolioTotalPositionResultModel
            {
                PortfolioId = portfolio.Id,
                TotalPosition = runningTotal,
                TotalCost = runningCost,
                TotalExpenses = runningExpense,
                CurrencyCode = portfolio.BankAccount.CurrencyCode,
                CurrencySymbol = portfolio.BankAccount.Currency.Symbol
            };
        }
        catch
        {
            return null;
        }
    }
}