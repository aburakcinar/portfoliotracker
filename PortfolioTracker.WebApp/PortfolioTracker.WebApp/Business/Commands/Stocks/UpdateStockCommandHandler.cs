using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.Stocks;

public sealed class UpdateStockCommand : IRequest<bool>
{
    public required string FullCode { get; init; }
    
    public required string Name { get; init; }
    
    public required string Description { get; init; }
    
    public required string LocaleCode { get; init; }

    public required string WebSite { get; init; }
} 

public sealed class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, bool>
{
    private readonly PortfolioContext m_context;

    public UpdateStockCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var stockItem = await m_context
                .StockItems
                .FirstOrDefaultAsync(s => s.FullCode == request.FullCode, cancellationToken);
            
            var locale = await m_context.Locales.FirstOrDefaultAsync(x => x.LocaleCode == request.LocaleCode);

            if (stockItem is null || locale is null)
            {
                return false;
            }
            
            stockItem.Name = request.Name;
            stockItem.Description = request.Description;
            stockItem.LocaleCode = request.LocaleCode;
            stockItem.Locale = locale;
            stockItem.WebSite = request.WebSite;

            var count = await m_context.SaveChangesAsync(cancellationToken);

            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}