using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Commands.PortfolioV2Entity;

public sealed class UpdatePortfolioV2Command : IRequest<bool>
{
    public Guid PortfolioId { get; init; }
    
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
    
    public Guid BankAccountId { get; init; }
}

public class UpdatePortfolioV2CommandHandler : IRequestHandler<UpdatePortfolioV2Command, bool>
{
    private readonly IPortfolioContext m_context;

    public UpdatePortfolioV2CommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(UpdatePortfolioV2Command request, CancellationToken cancellationToken)
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