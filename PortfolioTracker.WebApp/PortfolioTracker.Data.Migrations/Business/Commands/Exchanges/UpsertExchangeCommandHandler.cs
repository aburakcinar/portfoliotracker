using MediatR;
using PortfolioTracker.Data.Migrations.Services;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Commands.Exchanges;

public sealed class UpsertExchangeCommand : IRequest<bool>
{
    public required ExchangeItem Exchange { get; init; }
}

public sealed class UpsertExchangeCommandHandler : IRequestHandler<UpsertExchangeCommand, bool>
{
    private readonly IPortfolioContext m_context;

    public UpsertExchangeCommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(UpsertExchangeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var found = m_context.Exchanges.FirstOrDefault(x => x.Mic == request.Exchange.Mic);
            var item = Map(request.Exchange);
            
            if (found == null)
            {
                m_context.Exchanges.Add(item);
            }
            else
            {
                found = item;
            }
            
            var result = await m_context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
        catch
        {
            return false;
        }
    }

    private static Exchange Map(ExchangeItem item)
    {
        return new Exchange
        {
            Mic = item.Mic,
            OperatingMic = item.OperatingMic,
            OprtSgmt = item.OprtSgmt,
            MarketNameInstitutionDescription = item.MarketNameInstitutionDescription,
            LegalEntityName = item.LegalEntityName,
            Lei = item.Lei,
            MarketCategoryCode = item.MarketCategoryCode,
            Acronym = item.Acronym,
            CountryCode = item.CountryCode,
            City = item.City,
            WebSite = item.WebSite,
            Status = item.Status,
            CreationDate = item.CreationDate?.ToUniversalTime(),
            LastUpdateDate = item.LastUpdateDate?.ToUniversalTime(),
            LastValidationDate = item.LastValidationDate?.ToUniversalTime(),
            ExpiryDate = item.ExpiryDate?.ToUniversalTime(),
            Comments = item.Comments,
        };
    }
}

