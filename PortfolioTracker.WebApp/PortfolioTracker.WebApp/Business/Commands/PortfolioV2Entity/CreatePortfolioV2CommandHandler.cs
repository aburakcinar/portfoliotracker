using MediatR;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.PortfolioV2Entity;

public sealed class CreatePortfolioV2Command : IRequest<bool>
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}

public sealed class CreatePortfolioV2CommandHandler : IRequestHandler<CreatePortfolioV2Command, bool>
{
    private readonly PortfolioContext m_context;

    public CreatePortfolioV2CommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(CreatePortfolioV2Command request, CancellationToken cancellationToken)
    {
        try
        {
            m_context.PortfolioV2s.Add(new PortfolioV2
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
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