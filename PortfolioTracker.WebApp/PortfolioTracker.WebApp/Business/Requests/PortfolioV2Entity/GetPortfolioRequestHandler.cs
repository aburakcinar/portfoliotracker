using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.PortfolioV2Entity;

public sealed class GetPortfolioRequest : IRequest<PortfolioModel?>
{
    public Guid PortfolioId { get; init; }
}

public sealed class GetPortfolioRequestHandler : IRequestHandler<GetPortfolioRequest, PortfolioModel?>
{
    private readonly PortfolioContext m_context;

    public GetPortfolioRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<PortfolioModel?> Handle(GetPortfolioRequest request, CancellationToken cancellationToken)
    {
        var result = await m_context.PortfolioV2s.FirstOrDefaultAsync(x => x.Id == request.PortfolioId, cancellationToken);

        if (result is null)
        {
            return null;
        }

        return new PortfolioModel
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            IsDefault = result.IsDefault,
            Created = result.Created,
        };
    }
}