using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands;

public sealed class ReportSellCommand : IRequest<bool>
{
    public Guid HoldingId { get; init; }
    
    public decimal Price { get; init; }
    
    public decimal Quantity { get; init; }
    
    public decimal Expenses { get; init; }
    
    public DateTime ExecuteDate { get; init; }

}

public class ReportSellCommandHandler : IRequestHandler<ReportSellCommand, bool>
{
    private readonly PortfolioContext m_context;

    public ReportSellCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(ReportSellCommand request, CancellationToken cancellationToken)
    {
        var holding = await m_context
            .Holdings
            .Include(x => x.TransactionGroup)
            .ThenInclude(group => group.Transactions)
            .FirstOrDefaultAsync(x => x.Id == request.HoldingId, cancellationToken);

        if (holding == null)
        {
            return false;
        }

        var transactions = holding.TransactionGroup.Transactions;
        var buyTransaction = transactions.FirstOrDefault(x => x is { InOut: InOut.In, Type: TransactionType.Investment});

        if (buyTransaction == null)
        {
            return false;
        }
        
        // sell transaction
        m_context.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            Created = request.ExecuteDate,
            Type = TransactionType.Investment,
            Price = request.Price,
            Quantity = request.Quantity > buyTransaction.Quantity ?  buyTransaction.Quantity :request.Quantity,
            InOut = InOut.Out,
            Description = string.Empty,
            TransactionGroupId = holding.TransactionGroupId,
        });
        
        // sell expenses
        m_context.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            Created = request.ExecuteDate,
            Type = TransactionType.InterestCommission,
            Price = request.Expenses,
            Quantity = 1M,
            InOut = InOut.Out,
            Description = string.Empty,
            TransactionGroupId = holding.TransactionGroupId,
        });
        
        await m_context.SaveChangesAsync(cancellationToken);

        return true;
    }
    
}