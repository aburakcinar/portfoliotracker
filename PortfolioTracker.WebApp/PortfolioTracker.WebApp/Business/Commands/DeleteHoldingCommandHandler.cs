using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands;

public sealed class DeleteHoldingCommand : IRequest<bool>
{
    public Guid HoldingId { get; init; }
}

public class DeleteHoldingCommandHandler : IRequestHandler<DeleteHoldingCommand, bool>
{
    private readonly PortfolioContext m_context;

    public DeleteHoldingCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(DeleteHoldingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var found = await m_context
                .Holdings
                .Include(x => x.TransactionGroup)
                .ThenInclude(transactionGroup => transactionGroup.Transactions)
                .FirstOrDefaultAsync(x => x.Id == request.HoldingId, cancellationToken);

            if (found == null)
            {
                return false;
            }
        
            m_context.Holdings.Remove(found);
        
            await m_context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch 
        {
            return false;
        }
    }
}