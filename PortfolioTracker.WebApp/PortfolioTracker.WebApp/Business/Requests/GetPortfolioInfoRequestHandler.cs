using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class PortfolioInfoModel
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;
    
    public string CurrencyName { get; init; } = string.Empty;

    public string CurrencySymbol { get; init; } = string.Empty;
}

public sealed class GetPortfolioInfoRequest : IRequest<PortfolioInfoModel?>
{
    public Guid PortfolioId { get; init; }
}

public class GetPortfolioInfoRequestHandler : IRequestHandler<GetPortfolioInfoRequest, PortfolioInfoModel?>
{
    private readonly PortfolioContext m_context;
    
    public GetPortfolioInfoRequestHandler(PortfolioContext mContext)
    {
        m_context = mContext;
    }
    
    public async Task<PortfolioInfoModel?> Handle(GetPortfolioInfoRequest request, CancellationToken cancellationToken)
    {
        var portfolio = await m_context
            .Portfolios
            .Include(x => x.Currency)
            .FirstOrDefaultAsync(x => x.Id == request.PortfolioId);

        if (portfolio is null)
        {
            return null;
        }

        return new PortfolioInfoModel
        {
            Id = portfolio.Id,
            Name = portfolio.Name,
            Description = portfolio.Description,
            CurrencyCode = portfolio.CurrencyCode,
            CurrencyName = portfolio.Currency?.Name ?? string.Empty,
            CurrencySymbol = portfolio.Currency?.Symbol ?? string.Empty,
        };
    }
}