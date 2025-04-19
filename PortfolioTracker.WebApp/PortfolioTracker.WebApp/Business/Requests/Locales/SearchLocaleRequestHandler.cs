using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Requests.Locales;

public sealed class SearchLocaleRequest : IRequest<IEnumerable<LocaleQueryModel>>
{
    public required string SearchText { get; init; }
    
    public int Limit { get; init; }
}

public sealed class SearchLocaleRequestHandler : IRequestHandler<SearchLocaleRequest, IEnumerable<LocaleQueryModel>>
{
    private readonly IPortfolioContext m_context;

    public SearchLocaleRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<LocaleQueryModel>> Handle(SearchLocaleRequest request,
        CancellationToken cancellationToken)
    {
        var searchText = request.SearchText.ToLower();

        return await m_context
            .Locales
            .Where(x =>
                x.LocaleCode == searchText ||
                x.CountryName.ToLower().Contains(searchText) ||
                x.CountryCode == searchText ||
                x.CurrencyName.ToLower().Contains(searchText) ||
                x.CurrencyCode == searchText)
            .Take(request.Limit)
            .Select(item => new LocaleQueryModel
            {
                LocaleCode = item.LocaleCode,
                LanguageName = item.LanguageName,
                CountryName = item.CountryName,
                CountryCode = item.CountryCode,
                CurrencyCode = item.CurrencyCode,
                CurrencyName = item.CurrencyName,
                CurrencySymbol = item.CurrencySymbol,
            }).ToListAsync(cancellationToken);
    }
}