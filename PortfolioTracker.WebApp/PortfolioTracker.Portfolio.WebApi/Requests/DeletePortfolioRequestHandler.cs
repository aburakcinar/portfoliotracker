using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Portfolio.WebApi.Requests;

public static class DeletePortfolioEndpointExtensions
{
    public static void MapDeletePortfolio(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", DeletePortfolio)
           .WithName(nameof(DeletePortfolio));
    }

    private static async Task<IResult> DeletePortfolio(IMediator mediator, Guid id)
    {
        var result = await mediator.Send(new DeletePortfolioRequest { PortfolioId = id });
        
        if (result)
        {
            return TypedResults.NoContent();
        }
        
        return TypedResults.NotFound();
    }
}

public sealed class DeletePortfolioRequest : IRequest<bool>
{
    public Guid PortfolioId { get; init; }
}

public sealed class DeletePortfolioRequestHandler : IRequestHandler<DeletePortfolioRequest, bool>
{
    private readonly IPortfolioContext m_context;

    public DeletePortfolioRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(DeletePortfolioRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var portfolio = await m_context.Portfolios
                .FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

            if (portfolio is null)
            {
                return false;
            }
            
            // Check if there are any related holdings before deleting
            var hasHoldings = await m_context.Holdings
                .AnyAsync(h => h.PortfolioId == request.PortfolioId, cancellationToken);
                
            if (hasHoldings)
            {
                // Cannot delete a portfolio with holdings
                return false;
            }
            
            m_context.Portfolios.Remove(portfolio);
            var count = await m_context.SaveChangesAsync(cancellationToken);
            
            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}
