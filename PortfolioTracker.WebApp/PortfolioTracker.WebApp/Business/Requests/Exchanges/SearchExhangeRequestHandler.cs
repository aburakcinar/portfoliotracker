using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.Exchanges;

public sealed class SearchExhangeRequest : IRequest<IEnumerable<ExchangeQueryModel>>
{
    public required string SearchText { get; init; } 
    
    public int Limit { get; init; }
}

public sealed class SearchExhangeRequestHandler : IRequestHandler<SearchExhangeRequest, IEnumerable<ExchangeQueryModel>>
{
    private readonly PortfolioContext m_context;

    public SearchExhangeRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<ExchangeQueryModel>> Handle(SearchExhangeRequest request, CancellationToken cancellationToken)
    {
        var searchText = request.SearchText.ToLowerInvariant();

        var result = await m_context.Exchanges.Where(x =>
                x.CountryCode == searchText ||
                x.Mic.ToLower().StartsWith(searchText) ||
                x.LegalEntityName.ToLower().Contains(searchText) ||
                x.MarketNameInstitutionDescription.ToLower().Contains(searchText))
            .Take(request.Limit)
            .Select(x => new ExchangeQueryModel
            {
                Mic = x.Mic,
                LegalEntityName = x.LegalEntityName,
                MarketNameInstitutionDescription = x.MarketNameInstitutionDescription,
                City = x.City,
                CountryCode = x.CountryCode,
            })
            .ToListAsync(cancellationToken);
        
        return result;
    }
}