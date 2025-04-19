using MediatR;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Commands.PortfolioV2Entity;

public sealed class CreatePortfolioV2Command : IRequest<bool>
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
    
    public Guid BankAccountId { get; init; }
}

public sealed class CreatePortfolioV2CommandHandler : IRequestHandler<CreatePortfolioV2Command, bool>
{
    private readonly IPortfolioContext m_context;

    public CreatePortfolioV2CommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(CreatePortfolioV2Command request, CancellationToken cancellationToken)
    {
        try
        {
            m_context.Portfolios.Add(new Portfolio
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                BankAccountId = request.BankAccountId,
                Created = DateTime.Now.ToUniversalTime(),
                IsDefault = false
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