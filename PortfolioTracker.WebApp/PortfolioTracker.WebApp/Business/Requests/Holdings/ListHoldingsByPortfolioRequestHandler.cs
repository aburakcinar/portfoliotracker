using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.HoldingV2Entity;

public sealed class ListHoldingsByPortfolioRequest : IRequest<IEnumerable<HoldingAggregateModel>>
{
    public Guid PortfolioId { get; init; }
}

public sealed class
    ListHoldingsByPortfolioRequestHandler : IRequestHandler<ListHoldingsByPortfolioRequest,
    IEnumerable<HoldingAggregateModel>>
{
    private readonly PortfolioContext m_context;

    public ListHoldingsByPortfolioRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<HoldingAggregateModel>> Handle(ListHoldingsByPortfolioRequest request,
        CancellationToken cancellationToken)
    {
        var items = await m_context
            .Holdings
            .Include(x => x.Portfolio)
            .ThenInclude(portfolio => portfolio.BankAccount)
            .ThenInclude(bankAccount => bankAccount.Currency)
            .Include(x => x.Asset)
            .ThenInclude(asset => asset.Exchange)
            .Include(x => x.BankAccountTransactionGroup)
            .ThenInclude(transactionGroup => transactionGroup.Transactions)
            .Where(x => x.PortfolioId == request.PortfolioId)
            .ToListAsync(cancellationToken);


        return items
            .GroupBy(x => x.AssetId)
            .Select(Aggregate);
    }

    private HoldingAggregateModel Aggregate(IGrouping<Guid, Holding> group)
    {
        var first = group.First();

        var transactions = group.SelectMany(x => x.BankAccountTransactionGroup.Transactions);

        decimal totalQuantity = 0;
        decimal totalCost = 0;
        decimal totalExpenses = 0;

        foreach (var holding in group)
        {
            foreach (var transaction in holding.BankAccountTransactionGroup.Transactions)
            {
                if (transaction is { ActionTypeCode: @"BUY_ASSET" })
                {
                    totalQuantity += transaction.Quantity;
                    totalCost += (transaction.Quantity * transaction.Price);
                }
                else if (transaction is { ActionTypeCode: @"SELL_ASSET" })
                {
                    totalQuantity -= transaction.Quantity;
                    totalCost -= (transaction.Quantity * transaction.Price);
                }
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
            CurrencyCode = first.Portfolio.BankAccount.CurrencyCode,
            CurrencyName = first.Portfolio.BankAccount.Currency.Name,
            CurrencySymbol = first.Portfolio.BankAccount.Currency.Symbol,

            TotalQuantity = totalQuantity,
            TotalCost = totalCost,
            AveragePrice = totalQuantity > 0 ? totalCost / totalQuantity : 0,
            TotalExpenses = totalExpenses
        };
    }
}