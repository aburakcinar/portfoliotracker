using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Data.Migrations.Business.Commands;

public sealed class ImportCurrenciesFromLocalesCommand : IRequest<bool>
{
}

public sealed class ImportCurrenciesFromLocalesCommandHandler : IRequestHandler<ImportCurrenciesFromLocalesCommand, bool>
{
    private readonly ILogger<ImportCurrenciesFromLocalesCommandHandler> m_logger;
    private readonly IPortfolioContext m_context;

    public ImportCurrenciesFromLocalesCommandHandler(
        ILogger<ImportCurrenciesFromLocalesCommandHandler> logger,
        IPortfolioContext context)
    {
        m_logger = logger;
        m_context = context;
    }

    public async Task<bool> Handle(ImportCurrenciesFromLocalesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            m_logger.LogInformation("Start importing Currencies...");

            var currencies = await m_context.Locales.GroupBy(x => x.CurrencyCode).ToListAsync(cancellationToken);

            foreach (var localeGroup in currencies)
            {
                var currencyItem =
                    await m_context.Currencies.FirstOrDefaultAsync(x => x.Code == localeGroup.Key,
                        cancellationToken);

                var currencyFromLocale = localeGroup.First();

                if (currencyItem is not null)
                {
                    currencyItem.Name = currencyFromLocale.CurrencyName;
                    currencyItem.NameLocal = currencyFromLocale.CurrencyNameLocal;
                    currencyItem.Symbol = currencyFromLocale.CurrencySymbol;
                    currencyItem.SubunitValue = currencyFromLocale.CurrencySubunitValue;
                    currencyItem.SubunitName = currencyFromLocale.CurrencySubunitName;
                }
                else
                {
                    var currencyNewItem = new Currency
                    {
                        Code = currencyFromLocale.CurrencyCode,
                        Name = currencyFromLocale.CurrencyName,
                        NameLocal = currencyFromLocale.CurrencyNameLocal,
                        Symbol = currencyFromLocale.CurrencySymbol,
                        SubunitValue = currencyFromLocale.CurrencySubunitValue,
                        SubunitName = currencyFromLocale.CurrencySubunitName
                    };

                    m_context.Currencies.Add(currencyNewItem);
                }
            }

            await m_context.SaveChangesAsync(cancellationToken);

            m_logger.LogInformation("Ended importing Currencies.");

            return true;
        }
        catch(Exception ex) 
        {
            m_logger.LogError(message: "Error on importing Currencies.", exception: ex);
            return false;
        }
    }
}