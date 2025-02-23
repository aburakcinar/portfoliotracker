using System.Globalization;
using System.Xml;
using FinanceData.Business.Api;

namespace FinanceData.Business.Services;

internal sealed class EcbExchangeRateProvider : BaseRateProvider
{
    private readonly IHttpClientFactory m_httpClientFactory;

    public EcbExchangeRateProvider(IHttpClientFactory httpClientFactory)
    {
        m_httpClientFactory = httpClientFactory;
    }

    public override async Task<decimal> GetRateAsync(CurrencyRateQueryModel query)
    {
        using var httpClient = m_httpClientFactory.CreateClient();

        var ratesToEuro = new List<CurrencyRateResult>();

        try
        {
            var stream = await httpClient.GetStreamAsync("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");

            //load XML document
            var document = new XmlDocument();
            document.Load(stream);

            //add namespaces
            var namespaces = new XmlNamespaceManager(document.NameTable);
            namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
            namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

            //get daily rates
            var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
            if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None,
                    out var updateDate))
                updateDate = DateTime.UtcNow;

            foreach (XmlNode currency in dailyRates.ChildNodes)
            {
                //get rate
                if (!decimal.TryParse(currency.Attributes["rate"].Value, NumberStyles.Currency,
                        CultureInfo.InvariantCulture, out var currencyRate))
                    continue;

                ratesToEuro.Add(new CurrencyRateResult()
                {
                    BaseCurrency = @TargetCurrencies.EUR,
                    TargetCurrency = currency.Attributes["currency"].Value.ToUpper(),
                    Rate = currencyRate,
                    Date = DateOnly.FromDateTime(updateDate)
                });
            }

            return ratesToEuro.FirstOrDefault(x => x.TargetCurrency == query.Target)?.Rate ?? Undefined.Decimal;
        }
        catch
        {
            return Undefined.Decimal;
        }
    }
}