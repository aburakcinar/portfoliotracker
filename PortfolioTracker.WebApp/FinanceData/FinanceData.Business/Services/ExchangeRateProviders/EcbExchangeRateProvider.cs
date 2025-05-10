//using System.Globalization;
//using System.Xml;
//using FinanceData.Business.Api;
//using FinanceData.Business.DataStore;
//using FinanceData.Business.Extensions;
//using FinanceData.Business.Utils;

//namespace FinanceData.Business.Services.ExchangeRateProviders;

//internal sealed class EcbExchangeRateProvider : BaseRateProvider
//{
//    private readonly IHttpClientFactory m_httpClientFactory;
//    private readonly ICurrencyExchangeRateIngressService m_currencyExchangeRateIngressService;

//    public EcbExchangeRateProvider(
//        IHttpClientFactory httpClientFactory,
//        ICurrencyExchangeRateIngressService currencyExchangeRateIngressService)
//    {
//        m_httpClientFactory = httpClientFactory;
//        m_currencyExchangeRateIngressService = currencyExchangeRateIngressService;
//    }

//    public override int Order => 100;

//    public override async Task<decimal> GetRateAsync(CurrencyRateQueryModel query)
//    {
//        using var httpClient = m_httpClientFactory.CreateClient();

//        var ratesToEuro = new List<CurrencyExchangeRatio>();

//        try
//        {
//            var stream = await httpClient.GetStreamAsync("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");

//            //load XML document
//            var document = new XmlDocument();
//            document.Load(stream);

//            //add namespaces
//            var namespaces = new XmlNamespaceManager(document.NameTable);
//            namespaces.AddNamespace("ns", @"http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
//            namespaces.AddNamespace("gesmes", @"http://www.gesmes.org/xml/2002-08-01");

//            //get daily rates
//            var dailyRates = document.SelectSingleNode(@"gesmes:Envelope/ns:Cube/ns:Cube", namespaces);

//            if (dailyRates is { Attributes: { } })
//            {
//                foreach (XmlNode currency in dailyRates.ChildNodes)
//                {
//                    var targetCurrency = currency.Get(@"currency").ToUpper();
//                    var updateDate = dailyRates.GetDateTime(@"time");
//                    var rate = currency.GetAsDecimal(@"rate");

//                    if (string.IsNullOrEmpty(targetCurrency) || updateDate is null || rate == Undefined.Decimal)
//                    {
//                        continue;
//                    }
                    
//                    ratesToEuro.Add(new CurrencyExchangeRatio()
//                    {
//                        BaseCurrency = @TargetCurrencies.EUR,
//                        TargetCurrency = targetCurrency,
//                        Rate = rate,
//                        Date = updateDate.Value.ToUniversalTime()
//                    });
//                }

//                await m_currencyExchangeRateIngressService.IngressAsync(ratesToEuro);
//            }

//            return ratesToEuro.FirstOrDefault(x => x.TargetCurrency == query.Target)?.Rate ?? Undefined.Decimal;
//        }
//        catch
//        {
//            return Undefined.Decimal;
//        }
//    }
//}