using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Portfolio.WebApi.Requests;

public static class UpdatePortfolioEndpointExtensions
{
    public static void MapUpdatePortfolio(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}", UpdatePortfolio)
           .WithName(nameof(UpdatePortfolio));
    }

    private static async Task<IResult> UpdatePortfolio(IMediator mediator, Guid id, UpdatePortfolioRequest request)
    {
        if (id != request.PortfolioId)
        {
            return TypedResults.BadRequest("ID in route must match ID in request body");
        }
        
        var result = await mediator.Send(request);
        
        if (result)
        {
            return TypedResults.NoContent();
        }
        
        return TypedResults.NotFound();
    }
}

public sealed class UpdatePortfolioRequest : IRequest<bool>
{
    public Guid PortfolioId { get; init; }
    
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
    
    public Guid BankAccountId { get; init; }
}

public class UpdatePortfolioRequestHandler : IRequestHandler<UpdatePortfolioRequest, bool>
{
    private readonly IPortfolioContext m_context;

    public UpdatePortfolioRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(UpdatePortfolioRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await m_context.Portfolios.FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

            if (item is null)
            {
                return false;
            }
            
            item.Name = request.Name;
            item.Description = request.Description;
            item.BankAccountId = request.BankAccountId;
            
            var count = await m_context.SaveChangesAsync(cancellationToken);
            
            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}
