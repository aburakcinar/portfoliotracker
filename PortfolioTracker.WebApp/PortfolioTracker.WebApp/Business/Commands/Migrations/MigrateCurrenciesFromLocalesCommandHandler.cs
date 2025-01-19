using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.Migrations;

public sealed class MigrateCurrenciesFromLocalesCommand : IRequest<bool>
{
    
}

public sealed class MigrateCurrenciesFromLocalesCommandHandler : IRequestHandler<MigrateCurrenciesFromLocalesCommand, bool>
{
    private readonly PortfolioContext m_context;

    public MigrateCurrenciesFromLocalesCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(MigrateCurrenciesFromLocalesCommand request, CancellationToken cancellationToken)
    {
        try
        {
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

            return true;
        }
        catch
        {
            return false;
        }
    }
}