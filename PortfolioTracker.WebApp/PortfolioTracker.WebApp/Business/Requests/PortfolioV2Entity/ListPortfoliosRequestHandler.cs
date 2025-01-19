using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.PortfolioV2Entity;

public sealed class PortfolioModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public bool IsDefault { get; init; }

    public DateTime Created { get; init; }
}

public sealed class ListPortfoliosRequest : IRequest<IEnumerable<PortfolioModel>>
{
}

public sealed class ListPortfoliosRequestHandler : IRequestHandler<ListPortfoliosRequest, IEnumerable<PortfolioModel>>
{
    private readonly PortfolioContext m_context;

    public ListPortfoliosRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<PortfolioModel>> Handle(ListPortfoliosRequest request,
        CancellationToken cancellationToken)
    {
        return await m_context
            .PortfolioV2s
            .Select(x => new PortfolioModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsDefault = x.IsDefault,
                Created = x.Created
            }).ToListAsync(cancellationToken);
    }
}