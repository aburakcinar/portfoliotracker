using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands;

public sealed class UpdateHoldingCommand : IRequest<bool>
{
    public Guid PortfolioId { get; init; }
    
    public required string StockSymbol { get; init; }
    
    public Guid HoldingId { get; init; }
    
    public decimal Quantity { get; init; }
   
    public decimal Price { get; init; }

    public decimal Expenses { get; init; }
   
    public DateTime? ExecuteDate { get; init; }
}

public sealed class UpdateHoldingCommandHandler : IRequestHandler<UpdateHoldingCommand, bool>
{
    private readonly PortfolioContext m_context;

    public UpdateHoldingCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(UpdateHoldingCommand request, CancellationToken cancellationToken)
    {
        var holding = await m_context
            .Holdings
            .Include(x => x.TransactionGroup)
            .ThenInclude(transactionGroup => transactionGroup.Transactions)
            .FirstOrDefaultAsync(x => 
                x.Id == request.HoldingId && 
                x.PortfolioId == request.PortfolioId, cancellationToken);

        if (holding == null)
        {
            return false;
        }
        
        var buyTransaction =
            holding.TransactionGroup.Transactions.FirstOrDefault(x => x is
                { Type: TransactionType.Investment, InOut: InOut.In });
        var expenseTransaction =
            holding.TransactionGroup.Transactions.FirstOrDefault(x => x is
                { Type: TransactionType.InterestCommission, InOut: InOut.In });

        if (buyTransaction is not null && expenseTransaction is not null)
        {
            buyTransaction.Price = request.Price;
            buyTransaction.Quantity = request.Quantity;
            
            expenseTransaction.Price = request.Expenses;
            expenseTransaction.Quantity = 1M;
            
            if (request.ExecuteDate.HasValue)
            {
                buyTransaction.Created = request.ExecuteDate.Value;
                expenseTransaction.Created = request.ExecuteDate.Value;
            }
            
            await m_context.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        return false;
    }
}