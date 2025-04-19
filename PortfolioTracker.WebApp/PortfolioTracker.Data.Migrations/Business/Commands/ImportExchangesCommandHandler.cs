using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Migrations.Services;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Data.Migrations.Business.Commands;

public sealed class ImportExchangesCommand : IRequest<bool>
{

}

public sealed class ImportExchangesCommandHandler : IRequestHandler<ImportExchangesCommand, bool>
{
    private readonly ILogger<ImportExchangesCommandHandler> m_logger;
    private readonly IExchangeReader m_exchangeReader;
    private readonly IPortfolioContext m_context;

    public ImportExchangesCommandHandler(
        ILogger<ImportExchangesCommandHandler> logger,
        IExchangeReader exchangeReader,
        IPortfolioContext context
        )
    {
        m_logger = logger;
        m_exchangeReader = exchangeReader;
        m_context = context;
    }

    public async Task<bool> Handle(ImportExchangesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            m_logger.LogInformation("Start importing Exchanges...");

            List<ExchangeItem> itemsToImportAll = m_exchangeReader.List().ToList();

            if (itemsToImportAll is null)
            {
                return false;
            }


            List<string> itemsExists = await m_context
                .Exchanges
                .Select(x => x.Mic)
                .ToListAsync(cancellationToken);

            var itemsToImport = itemsToImportAll
                .Where(x => !itemsExists.Contains(x.Mic))
                .Select(Map)
                .ToArray();

            m_context.Exchanges.AddRange(itemsToImport);

            var result = await m_context.SaveChangesAsync(cancellationToken);

            m_logger.LogInformation($@"End importing Exchanges with {result} items.");

            return result > 0;
        }
        catch (Exception ex)
        {
            m_logger.LogError(message: "Error on importing Exchanges", exception: ex);
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
