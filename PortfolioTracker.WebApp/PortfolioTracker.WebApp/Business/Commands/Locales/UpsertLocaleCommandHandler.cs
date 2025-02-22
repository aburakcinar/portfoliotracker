using MediatR;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Business.Commands.Locales;

public sealed class UpsertLocaleCommand : IRequest<bool>
{
    public required LocaleItem Locale { get; init; }
}

public sealed class UpsertLocaleCommandHandler : IRequestHandler<UpsertLocaleCommand, bool> 
{
    private readonly PortfolioContext m_context;

    public UpsertLocaleCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(UpsertLocaleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var found = m_context.Locales.FirstOrDefault(x => x.LocaleCode == request.Locale.Locale);
            var item = Map(request.Locale);
            
            if (found is null)
            {
                m_context.Locales.Add(item);
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