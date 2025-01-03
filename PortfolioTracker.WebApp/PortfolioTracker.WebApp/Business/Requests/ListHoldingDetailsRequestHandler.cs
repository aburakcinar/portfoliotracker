using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class HoldingDetailModel
{
    public Guid Id { get; init; }
    
    public decimal Quantity { get; init; }
    
    public decimal Price { get; init; }
    
    public decimal Expenses { get; init; }
    
    public DateTime ExecuteDate { get; init; }
}

public sealed class ListHoldingDetailsRequest : IRequest<IEnumerable<HoldingDetailModel>>
{
    public Guid PortfolioId { get; init; }
    
    public required string StockSymbol { get; init; }
}

public sealed class ListHoldingDetailsRequestHandler : IRequestHandler<ListHoldingDetailsRequest, IEnumerable<HoldingDetailModel>>
{
    private readonly PortfolioContext m_context;

    public ListHoldingDetailsRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<HoldingDetailModel>> Handle(ListHoldingDetailsRequest request, CancellationToken cancellationToken)
    {
        var data = await m_context
            .Holdings
            .Include(x => x.TransactionGroup)
            .ThenInclude(transactionGroup => transactionGroup.Transactions)
            .Include(x => x.Stock)
            .Where(x => x.PortfolioId == request.PortfolioId && x.Stock.Symbol == request.StockSymbol)
            .ToListAsync(cancellationToken);

        return data
            .Select(holding =>
            {
                var buyTransaction = holding.TransactionGroup.Transactions.FirstOrDefault(x =>
                    x is { Type: TransactionType.Investment, InOut: InOut.In });
                var expenseTransaction = holding.TransactionGroup.Transactions.FirstOrDefault(x =>
                    x is { Type: TransactionType.InterestCommission, InOut: InOut.In });

                return new HoldingDetailModel
                {
                    Id = holding.Id,
                    Quantity = buyTransaction?.Quantity ?? 0,
                    Price = buyTransaction?.Price ?? 0,
                    Expenses = expenseTransaction?.Price ?? 0,
                    ExecuteDate = buyTransaction?.Created ?? DateTime.MinValue,
                };
            });

    }
}