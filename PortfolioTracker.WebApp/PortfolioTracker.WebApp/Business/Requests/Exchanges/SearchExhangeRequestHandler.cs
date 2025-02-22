using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Extensions;

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
        var currencyDic = m_context
            .Locales
            .Select(x => new {CountryCode = x.CountryCode, CurrencyCode = x.CurrencyCode})
            .ToList()
            .DistinctBy(x => x.CountryCode)
            .ToDictionary(x => x.CountryCode, x => x.CurrencyCode);

        var result = await m_context.Exchanges.Where(x =>
                x.CountryCode == searchText ||
                x.Mic.ToLower().StartsWith(searchText) ||
                x.LegalEntityName.ToLower().Contains(searchText) ||
                x.MarketNameInstitutionDescription.ToLower().Contains(searchText) ||
                x.Acronym.ToLower().Contains(searchText))
            .Take(request.Limit)
            .Select(x => new ExchangeQueryModel
            {
                Code = x.GetCode(),
                Mic = x.Mic,
                LegalEntityName = x.LegalEntityName,
                MarketNameInstitutionDescription = x.MarketNameInstitutionDescription,
                City = x.City,
                CountryCode = x.CountryCode,
                CurrencyCode = currencyDic[x.CountryCode]
            })
            .ToListAsync(cancellationToken);
        
        return result;
    }
}