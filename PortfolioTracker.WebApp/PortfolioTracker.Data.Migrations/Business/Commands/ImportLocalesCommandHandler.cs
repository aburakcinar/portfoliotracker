using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.Data.Migrations.Business.Commands;

public sealed class ImportLocalesCommand : IRequest<bool>
{
}

public sealed class ImportLocalesCommandHandler : IRequestHandler<ImportLocalesCommand, bool>
{
    private readonly ILogger<ImportLocalesCommandHandler> m_logger;
    private readonly ILocalesReader m_localesReader;
    private readonly IPortfolioContext m_context;

    public ImportLocalesCommandHandler(ILogger<ImportLocalesCommandHandler> logger, ILocalesReader localesReader, IPortfolioContext context)
    {
        m_logger = logger;
        m_localesReader = localesReader;
        m_context = context;
    }

    public async Task<bool> Handle(ImportLocalesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            m_logger.LogInformation("Start importing Locales...");

            var itemsToImportAll = await m_localesReader.ListAsync();

            var itemsExists = await m_context
                .Locales
                .Select(x => x.LocaleCode)
                .ToListAsync(cancellationToken);

            var itemsToImport = itemsToImportAll
                .Where(x => !itemsExists.Contains(x.Locale))
                .Select(Map)
                .ToArray();

            m_context.Locales.AddRange(itemsToImport);

            var result = await m_context.SaveChangesAsync(cancellationToken);

            m_logger.LogInformation($@"End importing Locales with {result} items.");

            return result > 0;
        }
        catch (Exception ex)
        {
            m_logger.LogError(message: "Error on importing Locales", exception: ex);
            return false;
        }
    }

    private static Locale Map(LocaleItem item)
    {
        return new Locale
        {
            LocaleCode = item.Locale,
            // Country
            CountryName = item.Country.Name,
            CountryNameLocal = item.Country.NameLocal,
            CountryCode = item.Country.Code,
            // Currency
            CurrencyName = item.Country.Currency,
            CurrencyNameLocal = item.Country.CurrencyLocal,
            CurrencyCode = item.Country.CurrencyCode,
            CurrencySymbol = item.Country.CurrencySymbol,
            CurrencySubunitValue = item.Country.CurrencySubunitValue,
            CurrencySubunitName = item.Country.CurrencySubunitName,
            // Language
            LanguageName = item.Language.Name,
            LanguageNameLocal = item.Language.NameLocal,
        };
    }
}
