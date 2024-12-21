using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class StockModel
{
    public Guid Id { get; init; }
    
    public string StockExchange { get; init; } = string.Empty;

    public string Symbol { get; init; } =string.Empty;
    
    public string Name { get; init; } = string.Empty;
}

public sealed class ListStockRequest : IRequest<IEnumerable<StockModel>>
{
    
}

public sealed class ListStockRequestHandler : IRequestHandler<ListStockRequest, IEnumerable<StockModel>>
{
    private readonly PortfolioContext m_context;

    public ListStockRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<StockModel>> Handle(ListStockRequest request, CancellationToken cancellationToken)
    {
        return await m_context
            .Stocks
            .Select(x => new StockModel
            {
                Id = x.Id,
                StockExchange = x.StockExchange,
                Name = x.Name,
                Symbol = x.Symbol
            }).ToListAsync(cancellationToken);
    }
}