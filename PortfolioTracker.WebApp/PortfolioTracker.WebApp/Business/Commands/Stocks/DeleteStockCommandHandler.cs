using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.Stocks;

public sealed class DeleteStockCommand : IRequest<bool>
{
    public required string FullCode { get; init; }
}

public sealed class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand, bool>
{
    private readonly PortfolioContext m_context;

    public DeleteStockCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var stock = await m_context.StockItems.FirstOrDefaultAsync(x => x.FullCode == request.FullCode);

            if (stock == null)
            {
                return false;
            }
            
            m_context.StockItems.Remove(stock);
            
            var count = await m_context.SaveChangesAsync(cancellationToken);
            
            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}