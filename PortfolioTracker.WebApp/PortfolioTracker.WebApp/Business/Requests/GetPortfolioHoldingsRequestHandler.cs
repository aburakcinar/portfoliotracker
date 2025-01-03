using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class PortfolioHoldingModel
{
    public required string StockSymbol { get; init; }
    
    public required decimal Quantity { get; init; }

    public required decimal AveragePrice { get; init; }
    
    public required decimal TotalExpenses { get; init; }
}

public sealed class GetPortfolioHoldingsRequest : IRequest<IEnumerable<PortfolioHoldingModel>>
{
    public Guid PortfolioId { get; init; }
}

public sealed class GetPortfolioHoldingsRequestHandler : IRequestHandler<GetPortfolioHoldingsRequest, IEnumerable<PortfolioHoldingModel>>
{
    private readonly PortfolioContext m_context;

    public GetPortfolioHoldingsRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }
    
    public async Task<IEnumerable<PortfolioHoldingModel>> Handle(GetPortfolioHoldingsRequest request, CancellationToken cancellationToken)
    {
        var holdings = await m_context
            .Holdings
            .Include(x => x.TransactionGroup)
            .ThenInclude(transactionGroup => transactionGroup.Transactions)
            .Include(x => x.Stock)
            .Where(x => x.PortfolioId == request.PortfolioId)
            .ToListAsync(cancellationToken);

        return holdings
            .GroupBy(x => x.Stock.Symbol)
            .Select(Convert);
    }

    private PortfolioHoldingModel Convert(IGrouping<string, Holding> holding)
    {
        var holdingSummaries = holding
            .Select(GetHoldingSummary)
            .ToList();
        
        var quantity = holdingSummaries.Sum(x => x.Quantity);
        var total  = holdingSummaries.Sum(x => x.AveragePrice * x.Quantity);
        var totalExpenses = holdingSummaries.Sum(x => x.TotalExpenses);

        return new PortfolioHoldingModel
        {
            StockSymbol = holding.Key,
            Quantity = quantity,
            AveragePrice = quantity == 0 ? 0 : total / quantity,
            TotalExpenses = totalExpenses
        };
    }
    
    private record HoldingSummary(decimal Quantity, decimal AveragePrice, decimal TotalExpenses);

    private HoldingSummary GetHoldingSummary(Holding holding)
    {
        var transactions = holding.TransactionGroup.Transactions;
        
        decimal quantity = 0;

        foreach (var transaction in transactions)
        {
            if (transaction is { Type: TransactionType.Investment, InOut: InOut.In })
            {
                quantity += transaction.Quantity;
            }else if (transaction is { Type: TransactionType.Investment, InOut: InOut.Out })
            {
                quantity -= transaction.Quantity;
            }
        }

        if (quantity < 0M)
        {
            quantity = 0M;
        }

        decimal price = transactions.FirstOrDefault(x => x is { Type: TransactionType.Investment, InOut: InOut.In })?.Price ?? 0M;
        
        decimal totalExpenses = transactions
            .Where(transaction => transaction is {Type: TransactionType.InterestCommission})
            .Sum(x => x.Price);
        
        return new HoldingSummary(quantity, price, totalExpenses);
    }
}