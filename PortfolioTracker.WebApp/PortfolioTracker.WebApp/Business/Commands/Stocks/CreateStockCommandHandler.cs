using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.Stocks;

public sealed class CreateStockCommand : IRequest<bool>
{
    public required string StockExchangeCode { get; init; }

    public required string Symbol { get; init; }
    
    public required string Name { get; init; }
    
    public required string Description { get; init; }
    
    public required string LocaleCode { get; init; }

    public required string WebSite { get; init; }
} 

public sealed class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, bool>
{
    private readonly PortfolioContext m_context;

    public CreateStockCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var stockExchange = await m_context.Exchanges.FirstOrDefaultAsync(x => x.Mic == request.StockExchangeCode);
            var locale = await m_context.Locales.FirstOrDefaultAsync(x => x.LocaleCode == request.LocaleCode);

            if (stockExchange is null || locale is null)
            {
                return false;
            }
            
            m_context.StockItems.Add(new StockItem
            {
                FullCode = $@"{request.StockExchangeCode}:{request.Symbol}",
                StockExchangeCode = request.StockExchangeCode,
                StockExchange = stockExchange,
                Symbol = request.Symbol,
                Name = request.Name,
                Description = request.Description,
                LocaleCode = request.LocaleCode,
                Locale = locale,
                WebSite = request.WebSite,
            });

            var count = await m_context.SaveChangesAsync(cancellationToken);

            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}