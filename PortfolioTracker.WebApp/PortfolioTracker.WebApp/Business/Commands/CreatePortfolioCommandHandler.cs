using MediatR;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands;

public sealed class CreatePortfolioCommand : IRequest<Guid>
{
    public required string Name { get; init; } 
    
    public required string CurrencyCode { get; init; } 
}

public class CreatePortfolioCommandHandler : IRequestHandler<CreatePortfolioCommand, Guid>
{
    private readonly PortfolioContext m_context;

    public CreatePortfolioCommandHandler(
        PortfolioContext mContext
        )
    {
        m_context = mContext;
    }
    
    public async Task<Guid> Handle(CreatePortfolioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Guid portfolioId = Guid.NewGuid();
        
            m_context.Portfolios.Add(new Portfolio
            {
                Id = portfolioId,
                Name = request.Name,
                CurrencyCode = request.CurrencyCode
            });
        
            await m_context.SaveChangesAsync(cancellationToken);
        
            return portfolioId;
        }
        catch 
        {
            return Guid.Empty;
        }
    }
}