using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.Stocks;

public sealed class ListStocksRequest : IRequest<IEnumerable<StockItemModel>>
{
}

public sealed class ListStocksRequestHandler : IRequestHandler<ListStocksRequest, IEnumerable<StockItemModel>>
{
    private readonly PortfolioContext m_context;

    public ListStocksRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<StockItemModel>> Handle(ListStocksRequest request,
        CancellationToken cancellationToken)
    {
        return await m_context
            .StockItems
            .Include(x => x.Locale)
            .Select(x => new StockItemModel
            {
                FullCode = x.FullCode,
                StockExchangeCode = x.StockExchangeCode,
                Symbol = x.Symbol,
                Name = x.Name,
                Description = x.Description,
                LocaleCode = x.LocaleCode,
                CurrencyCode = x.Locale.CurrencyCode,
                WebSite = x.WebSite
            }).ToListAsync(cancellationToken);
    }
}