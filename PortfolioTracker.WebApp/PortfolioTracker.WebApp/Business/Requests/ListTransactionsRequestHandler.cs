using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class TransactionViewModel
{
    public Guid Id { get; init; }
    
    public decimal Price { get; init; }
    
    public decimal Quantity { get; init; }
    
    public decimal Total { get; init; }
    
    public DateTime ExecuteDate { get; init; }
    
    public InOut InOut { get; init; }

    public TransactionType TransactionType { get; init; }
    
    public string Description { get; init; } = string.Empty;

    public Guid TransactionGroupId { get; init; }
    
    public Guid HoldingId { get; init; }
    
    public Guid PortfolioId { get; init; }
    
    public string PortfolioName { get; init; } = string.Empty;
    
    public string CurrencyCode { get; set; } = string.Empty;

    public Guid StockId { get; init; }

    public string StockName { get; set; }= string.Empty;
    
    public string StockSymbol { get; set; } = string.Empty;
}

public sealed class ListTransactionsRequest : IRequest<IEnumerable<TransactionViewModel>>
{
    
}

public class ListTransactionsRequestHandler: IRequestHandler<ListTransactionsRequest, IEnumerable<TransactionViewModel>>
{
    private readonly PortfolioContext m_context;

    public ListTransactionsRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<TransactionViewModel>> Handle(ListTransactionsRequest request, CancellationToken cancellationToken)
    {
        var transactions = await m_context
            .Transactions
            .Include(x => x.TransactionGroup)
            .ToListAsync(cancellationToken);

        var holdings = await m_context
            .Holdings
            .Include(x => x.Stock)
            .Include(x => x.Portfolio)
            .ThenInclude(portfolio => portfolio.Currency)
            .ToListAsync(cancellationToken);

        var result = from transaction in transactions
            join holding in holdings on transaction.TransactionGroupId equals holding.TransactionGroupId
            orderby transaction.Created descending 
            select new TransactionViewModel
            {
                Id = transaction.Id,
                Price = transaction.Price,
                Quantity = transaction.Quantity,
                Total = transaction.Price * transaction.Quantity,
                ExecuteDate = transaction.Created,
                InOut = transaction.InOut,
                TransactionType = transaction.Type,
                Description = transaction.Description,
                TransactionGroupId = transaction.TransactionGroupId,
                HoldingId = holding.Id,
                PortfolioId = holding.PortfolioId,
                PortfolioName = holding.Portfolio.Name,
                CurrencyCode = holding.Portfolio.CurrencyCode,
                StockId = holding.StockId,
                StockName = holding.Stock.Name,
                StockSymbol = holding.Stock.Symbol,
            };

        return result;
    }
}