using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests;

public sealed class PortfolioModel
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }= string.Empty;
    
    public string Description { get; init; }= string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;
    
    public string CurrencyName { get; init; } = string.Empty;
    
    public string CurrencySymbol { get; init; } = string.Empty;
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

    public async Task<IEnumerable<PortfolioModel>> Handle(ListPortfoliosRequest request, CancellationToken cancellationToken)
    {
        var result =  m_context
            .Portfolios
            .Include(x => x.Currency)
            .ToList()
            .Select(x =>
            {
                var currencyName = x.Currency?.Name ?? string.Empty;
                var currencySymbol = x.Currency?.Symbol ?? string.Empty;
                
                return new PortfolioModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CurrencyCode = x.CurrencyCode,
                    CurrencyName = currencyName,
                    CurrencySymbol = currencySymbol,
                };
            }).ToList();
        
        await Task.CompletedTask;
        
        return result;
    }
}