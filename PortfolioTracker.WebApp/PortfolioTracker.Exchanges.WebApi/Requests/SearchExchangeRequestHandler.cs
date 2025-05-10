using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Exchanges.WebApi.Models;
using PortfolioTracker.Exchanges.WebApp.Extensions;

namespace PortfolioTracker.Exchanges.WebApi.Requests;

public static class SearchExhangeEndpointExtension
{
    public static void MapSearchExhanges(this IEndpointRouteBuilder app)
    {
        app.MapGet(@"", SearchExhanges)
            .WithName(nameof(SearchExhanges));
    }

    private static async Task<IResult> SearchExhanges(IMediator mediator, string searchText, int limit = 10)
    {
        var result = await mediator.Send(new SearchExhangeRequest
        {
            SearchText = searchText,
            Limit = limit
        });
        return TypedResults.Ok(result);
    }
}

public sealed class SearchExhangeRequest : IRequest<IEnumerable<ExchangeQueryModel>>
{
    public required string SearchText { get; init; }

    public int Limit { get; init; }
}

public sealed class SearchExhangeRequestHandler : IRequestHandler<SearchExhangeRequest, IEnumerable<ExchangeQueryModel>>
{
    private readonly IPortfolioContext m_context;

    public SearchExhangeRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<ExchangeQueryModel>> Handle(SearchExhangeRequest request, CancellationToken cancellationToken)
    {
        var searchText = request.SearchText.ToLowerInvariant();
        var currencyDic = m_context
            .Locales
            .Select(x => new { CountryCode = x.CountryCode, CurrencyCode = x.CurrencyCode })
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