using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.HoldingV2Entity;

public sealed class HoldingDetailModel
{
    public Guid Id { get; init; }

    public InOut InOut { get; init; }

    public decimal Quantity { get; init; }

    public decimal Price { get; init; }

    public decimal Expenses { get; init; }

    public DateTime Created { get; init; }
}

public sealed class ListHoldingsByAssetRequest : IRequest<IEnumerable<HoldingDetailModel>>
{
    public Guid PortfolioId { get; init; }

    public Guid AssetId { get; init; }
}

public sealed class
    ListHoldingsByAssetRequestHandler : IRequestHandler<ListHoldingsByAssetRequest, IEnumerable<HoldingDetailModel>>
{
    private readonly PortfolioContext m_context;

    public ListHoldingsByAssetRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<HoldingDetailModel>> Handle(ListHoldingsByAssetRequest request,
        CancellationToken cancellationToken)
    {
        return (
                await m_context
                    .HoldingV2s
                    .Include(x => x.TransactionGroup)
                    .ThenInclude(transactionGroup => transactionGroup.Transactions)
                    .Where(x => x.PortfolioId == request.PortfolioId && x.AssetId == request.AssetId)
                    .ToListAsync(cancellationToken)
            )
            .Select(x =>
            {
                var buySellTransaction = x
                    .TransactionGroup
                    .Transactions
                    .First(p => p.Type == TransactionType.Investment);

                var expenseTransaction = x
                    .TransactionGroup
                    .Transactions
                    .First(p => p.Type == TransactionType.InterestCommission);

                return new HoldingDetailModel
                {
                    Id = x.Id,
                    InOut = buySellTransaction.InOut,
                    Created = x.Created,
                    Quantity = buySellTransaction.Quantity,
                    Price = buySellTransaction.Price,
                    Expenses = expenseTransaction.Price,
                };
            })
            .ToList();
    }
}