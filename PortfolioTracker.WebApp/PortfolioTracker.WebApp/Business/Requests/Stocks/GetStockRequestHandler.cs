using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.Stocks;

public sealed class GetStockRequest : IRequest<StockItemModel?>
{
    public required string FullCode { get; init; }
}

public sealed class GetStockRequestHandler : IRequestHandler<GetStockRequest, StockItemModel?>
{
    private readonly PortfolioContext m_context;

    public GetStockRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<StockItemModel?> Handle(GetStockRequest request, CancellationToken cancellationToken)
    {
        var stockItem = await m_context
            .StockItems
            .Include(x => x.Locale)
            .FirstOrDefaultAsync(x => x.FullCode == request.FullCode, cancellationToken);

        if (stockItem == null)
        {
            return null;
        }

        return new StockItemModel
        {
            FullCode = stockItem.FullCode,
            StockExchangeCode = stockItem.StockExchangeCode,
            Symbol = stockItem.Symbol,
            Name = stockItem.Name,
            Description = stockItem.Description,
            WebSite = stockItem.WebSite,
            LocaleCode = stockItem.LocaleCode,
            CurrencyCode = stockItem.Locale?.CurrencyCode ?? string.Empty,
        };
    }
}