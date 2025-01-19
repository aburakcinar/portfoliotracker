using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.HoldingV2Entity;

public sealed class ListHoldingsByPortfolioRequest : IRequest<IEnumerable<HoldingAggregateModel>>
{
    public Guid PortfolioId { get; init; }    
}

public sealed class ListHoldingsByPortfolioRequestHandler : IRequestHandler<ListHoldingsByPortfolioRequest, IEnumerable<HoldingAggregateModel>>
{
    private readonly PortfolioContext m_context;

    public ListHoldingsByPortfolioRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<HoldingAggregateModel>> Handle(ListHoldingsByPortfolioRequest request, CancellationToken cancellationToken)
    {
        var items = await m_context
            .HoldingV2s
            .Include(x => x.Asset)
            .ThenInclude(asset => asset.Exchange)
            .Include(x => x.Asset)
            .ThenInclude(asset => asset.Currency)
            .Include(x => x.TransactionGroup)
            .ThenInclude(transactionGroup => transactionGroup.Transactions)
            .Where(x => x.PortfolioId == request.PortfolioId)
            .ToListAsync(cancellationToken);


        return items
            .GroupBy(x => x.AssetId)
            .Select(Aggregate);


        // return items.Select(x => new HoldingListModel
        // {
        //     Id = x.Id,
        //     PortfolioId = x.PortfolioId,
        //     AssetId = x.AssetId,
        //     AssetName = x.Asset.Name,
        //     AssetTickerSymbol = x.Asset.TickerSymbol,
        //     AssetPrice = x.Asset.Price,
        //     AssetType = x.Asset.AssetType,
        //     AssetTypeName = x.Asset.AssetType.ToString(),
        //     ExchangeCode = x.Asset.ExchangeCode,
        //     CountryCode = x.Asset.Exchange.CountryCode,
        //     CurrencyCode = x.Asset.CurrencyCode,
        //     CurrencyName = x.Asset.Currency.Name,
        //     CurrencySymbol = x.Asset.Currency.Symbol,
        // });
    }

    private HoldingAggregateModel Aggregate(IGrouping<Guid, HoldingV2> group)
    {
        var first = group.First();

        var transactions = group.SelectMany(x => x.TransactionGroup.Transactions);

        decimal totalQuantity = 0;
        decimal totalCost = 0;
        decimal totalExpenses = 0;

        foreach (var transaction in transactions)
        {
            if (transaction is { InOut: InOut.In, Type: TransactionType.Investment })
            {
                totalQuantity += transaction.Quantity;
                totalCost += (transaction.Quantity * transaction.Price);
            } 
            else if (transaction is { InOut: InOut.Out, Type: TransactionType.Investment })
            {
                totalQuantity -= transaction.Quantity;
                totalCost -= (transaction.Quantity * transaction.Price);
            } 
            else if (transaction is {  Type: TransactionType.InterestCommission })
            {
                totalExpenses += transaction.Price * transaction.Quantity;
            } 
        }

        return new HoldingAggregateModel
        {
            PortfolioId = first.PortfolioId,
            AssetId = first.AssetId,
            AssetName = first.Asset.Name,
            AssetTickerSymbol = first.Asset.TickerSymbol,
            AssetPrice = first.Asset.Price,
            AssetType = first.Asset.AssetType,
            AssetTypeName = first.Asset.AssetType.ToString(),
            ExchangeCode = first.Asset.ExchangeCode,
            CountryCode = first.Asset.Exchange.CountryCode,
            CurrencyCode = first.Asset.CurrencyCode,
            CurrencyName = first.Asset.Currency.Name,
            CurrencySymbol = first.Asset.Currency.Symbol,

            TotalQuantity = totalQuantity,
            TotalCost = totalCost,
            AveragePrice = totalQuantity > 0 ? totalCost / totalQuantity : 0,
            TotalExpenses = totalExpenses
        };
    }
}