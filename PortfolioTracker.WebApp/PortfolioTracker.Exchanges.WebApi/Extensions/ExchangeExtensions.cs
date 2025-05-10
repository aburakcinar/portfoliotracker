using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Exchanges.WebApp.Extensions;

public static class ExchangeExtensions
{
    public static string GetName(this Exchange? exchange)
    {
        if (exchange is not null)
        {
            if (!string.IsNullOrWhiteSpace(exchange.Acronym))
            {
                return exchange.Acronym;
            }

            return exchange.MarketNameInstitutionDescription;
        }
        
        return string.Empty;
    }

    public static string GetCode(this Exchange? exchange)
    {
        if (exchange is not null)
        {
            if (!string.IsNullOrWhiteSpace(exchange.Acronym))
            {
                return exchange.Acronym;
            }

            return exchange.Mic;
        }
        
        return string.Empty;
    }
}