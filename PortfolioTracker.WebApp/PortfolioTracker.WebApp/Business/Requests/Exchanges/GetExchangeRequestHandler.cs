using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Extensions;

namespace PortfolioTracker.WebApp.Business.Requests.Exchanges;

public sealed class GetExchangeRequest : IRequest<ExchangeQueryModel?>
{
    public required string Mic { get; init; }
}

public sealed class GetExchangeRequestHandler : IRequestHandler<GetExchangeRequest, ExchangeQueryModel?>
{
    private readonly IPortfolioContext m_context;

    public GetExchangeRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<ExchangeQueryModel?> Handle(GetExchangeRequest request, CancellationToken cancellationToken)
    {
        var found = await m_context.Exchanges.FirstOrDefaultAsync(x => x.Mic == request.Mic, cancellationToken);

        if (found == null)
        {
            return null;
        }
        
        var locale = await m_context.Locales.FirstOrDefaultAsync(x => x.CountryCode == found.CountryCode, cancellationToken);

        return new ExchangeQueryModel
        {
            Code = found.GetCode(),
            Mic = found.Mic,
            LegalEntityName = found.LegalEntityName,
            MarketNameInstitutionDescription = found.MarketNameInstitutionDescription,
            City = found.City,
            CountryCode = found.CountryCode,
            CurrencyCode = locale?.CurrencyCode ?? string.Empty,
        };
    }
}