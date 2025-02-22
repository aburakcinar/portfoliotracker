using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.HoldingV2Entity;

public sealed class HoldingTransactionByExecuteDateModel
{
    public DateOnly ExecuteDate { get; init; }


    public List<HoldingTransactionGroupModel> Groups { get; init; } = new();
}

public sealed class HoldingTransactionGroupModel
{
    public Guid Id { get; init; }
    
    public required string Description { get; init; }

    public decimal Quantity { get; init; }
    
    public decimal Price { get; init; }
    
    public decimal Total { get; init; }
}

public sealed class ListHoldingTransactionsByAssetAndPortfolioRequest : IRequest<IEnumerable<HoldingTransactionByExecuteDateModel>>
{
    public Guid PortfolioId { get; init; }
    
    public Guid AssetId { get; init; }
}

public sealed class ListHoldingTransactionsByAssetAndPortfolioRequestHandler : IRequestHandler<ListHoldingTransactionsByAssetAndPortfolioRequest, IEnumerable<HoldingTransactionByExecuteDateModel>>
{
    private readonly PortfolioContext m_context;

    public ListHoldingTransactionsByAssetAndPortfolioRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<HoldingTransactionByExecuteDateModel>> Handle(
        ListHoldingTransactionsByAssetAndPortfolioRequest request, 
        CancellationToken cancellationToken
        )
    {
        var portfolio = m_context
            .Portfolios
            .Include(x => x.BankAccount)
            .FirstOrDefault(x => x.Id == request.PortfolioId);

        if (portfolio == null)
        {
            return Enumerable.Empty<HoldingTransactionByExecuteDateModel>();
        }
        
        var targetCurrencyCode = portfolio.BankAccount.CurrencyCode;

        var lst = m_context
            .Holdings
            .Include(x => x.BankAccountTransactionGroup)
            .ThenInclude(tg => tg.Transactions)
            .Where(h => h.PortfolioId == request.PortfolioId && h.AssetId == request.AssetId)
            .GroupBy(x => x.Created)
            .ToList();

        var result = lst.Select(ToModel);
        
        await Task.CompletedTask;
        
        return Enumerable.Empty<HoldingTransactionByExecuteDateModel>();
    }

    private HoldingTransactionByExecuteDateModel ToModel(IGrouping<DateTime, Holding>  item)
    {
        var model = new HoldingTransactionByExecuteDateModel
        {
            ExecuteDate = DateOnly.FromDateTime(item.Key),
            Groups = item.Select(ToModel).ToList()
        };
        
        return model;
    }

    private HoldingTransactionGroupModel ToModel(Holding item)
    {
        var buyTransaction = item
            .BankAccountTransactionGroup
            .Transactions
            .First(x => x is {ActionTypeCode: @"BUY_ASSET" });

        return new HoldingTransactionGroupModel
        {
            Id = item.Id,
            Description = item.BankAccountTransactionGroup.Description,
            Price = buyTransaction.Price,
            Quantity = buyTransaction.Quantity,
            Total = buyTransaction.Price * buyTransaction.Quantity
        };
    }
}