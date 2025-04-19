using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Requests.Locales;

public sealed class GetLocalByCountryCodeRequest : IRequest<LocaleQueryModel?>
{
    public required string CountryCode { get; init; }
}

public sealed class GetLocalByCountryCodeRequestHandler : IRequestHandler<GetLocalByCountryCodeRequest, LocaleQueryModel?>
{
    private readonly IPortfolioContext m_context;

    public GetLocalByCountryCodeRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<LocaleQueryModel?> Handle(GetLocalByCountryCodeRequest request, CancellationToken cancellationToken)
    {
        var item = await m_context.Locales.FirstOrDefaultAsync(x => x.CountryCode == request.CountryCode, cancellationToken);

        if (item == null)
        {
            return null;
        }

        return new LocaleQueryModel
        {
            LocaleCode = item.LocaleCode,
            LanguageName = item.LanguageName,
            CountryName = item.CountryName,
            CountryCode = item.CountryCode,
            CurrencyCode = item.CurrencyCode,
            CurrencyName = item.CurrencyName,
            CurrencySymbol = item.CurrencySymbol,
        };
    }
}