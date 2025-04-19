using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Requests.HoldingV2Entity;

public sealed class HoldingDetailModel
{
    public Guid Id { get; init; }

    public decimal Quantity { get; init; }

    public decimal Price { get; init; }

    public decimal Total { get; init; }
    
    public required string CurrencyCode { get; init; }

    public required string CurrencySymbol { get; init; }

    public DateOnly ExecuteDate { get; init; }
}

public sealed class ListHoldingsByAssetRequest : IRequest<IEnumerable<HoldingDetailModel>>
{
    public Guid PortfolioId { get; init; }

    public Guid AssetId { get; init; }
}

public sealed class ListHoldingsByAssetRequestHandler : IRequestHandler<ListHoldingsByAssetRequest, IEnumerable<HoldingDetailModel>>
{
    private readonly IPortfolioContext m_context;

    public ListHoldingsByAssetRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<HoldingDetailModel>> Handle(ListHoldingsByAssetRequest request,
        CancellationToken cancellationToken)
    {
        var portfolio =  await m_context
            .Portfolios
            .Include(x => x.BankAccount)
            .ThenInclude(bankAccount => bankAccount.Currency)
            .FirstAsync(x => x.Id == request.PortfolioId , cancellationToken);

        var currencyCode = portfolio.BankAccount.CurrencyCode;
        var currencySymbol = portfolio.BankAccount.Currency.Symbol;

        var lst = await m_context
            .Holdings
            .Include(x => x.BankAccountTransactionGroup)
            .ThenInclude(transactionGroup => transactionGroup.Transactions)
            .Where(x => x.PortfolioId == request.PortfolioId && x.AssetId == request.AssetId)
            .ToListAsync(cancellationToken);
        
        var asset = await m_context.Assets.FirstAsync(x => x.Id == request.AssetId, cancellationToken);

        return lst.Select(x =>
        {
            var quantity = GetHoldingQuantity(x);

            return new HoldingDetailModel
            {
                Id = x.Id,
                CurrencyCode = currencyCode,
                CurrencySymbol = currencySymbol,
                ExecuteDate = x.ExecuteDate,
                Quantity = quantity,
                Price = asset.Price,
                Total = asset.Price * quantity,
            };
        });
    }

    private decimal GetHoldingQuantity(Holding holding)
    {
        return holding
            .BankAccountTransactionGroup
            .Transactions
            .Find(x => x is {ActionTypeCode: @"BUY_ASSET"})?.Quantity ?? 0;
    }
    
}