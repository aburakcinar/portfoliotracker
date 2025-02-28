using System.Diagnostics;
using System.Globalization;
using System.Xml;
using FinanceData.Business.DataStore;

namespace FinanceData.Business.Services;

public interface IImportBulkService
{
    Task<bool> ExecuteAsync();
}

internal sealed class EcbImportBulkService : IImportBulkService
{
    private readonly ITargetCurrencyCodeService m_targetCurrencyCodeService;
    private readonly ICurrencyExchangeRateIngressService m_currencyExchangeRateIngressService;

    public EcbImportBulkService(
        ITargetCurrencyCodeService targetCurrencyCodeService,
        ICurrencyExchangeRateIngressService currencyExchangeRateIngressService)
    {
        m_targetCurrencyCodeService = targetCurrencyCodeService;
        m_currencyExchangeRateIngressService = currencyExchangeRateIngressService;
    }

    public async Task<bool> ExecuteAsync()
    {
        string filePath = @"C:\Users\abura\Downloads\eurofxref-sdmx.xml";

        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = XmlReader.Create(stream);

        var isDataset = false;
        var isGroup = false;
        var groupCurrency = string.Empty;
        var isSeries = false;
        var seriesCurrency = string.Empty;
        var seriesFrequency = string.Empty;

        var groupCurrencies = new List<string>();
        var seriesCurrencies = new List<string>();

        var lst = new List<CurrencyExchangeRatio>();

        Stopwatch sw = new Stopwatch();
        sw.Start();

        while (reader.Read())
        {
            if (reader is { NodeType: XmlNodeType.Element, Name: @"DataSet" })
            {
                isDataset = true;
            }

            if (isDataset && reader is { NodeType: XmlNodeType.Element, Name: @"Group" })
            {
                isGroup = true;
                groupCurrency = reader.GetAttribute(@"CURRENCY")?.ToString() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(groupCurrency))
                {
                    groupCurrencies.Add(groupCurrency);
                }
            }

            if (reader is { NodeType: XmlNodeType.EndElement, Name: @"Group" })
            {
                isGroup = false;
                groupCurrency = string.Empty;
            }

            if (isDataset && isGroup && reader is { NodeType: XmlNodeType.Element, Name: @"Series" })
            {
                seriesCurrency = reader.GetAttribute(@"CURRENCY")?.ToString() ?? string.Empty;

                if (m_targetCurrencyCodeService.IsValid(seriesCurrency))
                {
                    seriesCurrencies.Add(seriesCurrency);
                    isSeries = true;
                }
            }

            if (reader is { NodeType: XmlNodeType.EndElement, Name: @"Series" })
            {
                isSeries = false;
                seriesCurrency = string.Empty;
            }

            if (isDataset && isSeries && reader is { NodeType: XmlNodeType.Element, Name: @"Obs" })
            {
                if (DateTime.TryParseExact(
                        reader.GetAttribute(@"TIME_PERIOD")?.ToString(),
                        @"yyyy-MM-dd",
                        null,
                        DateTimeStyles.None, out var date)
                    && decimal.TryParse(
                        reader.GetAttribute(@"OBS_VALUE")?.ToString(),
                        out var rate))
                {
                    lst.Add(new CurrencyExchangeRatio
                    {
                        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                        BaseCurrency = TargetCurrencies.EUR,
                        TargetCurrency = seriesCurrency,
                        Rate = rate
                    });
                }
            }
        }

        await m_currencyExchangeRateIngressService.IngressAsync(lst);

        sw.Stop();

        Debug.WriteLine(@$"Total count :   {lst.Count} in {sw.ElapsedMilliseconds} ms");
        Debug.WriteLine($@"Group Currencies =>  {groupCurrencies.Count}, [{string.Join(",", groupCurrencies)}]");
        Debug.WriteLine($@"Series Currencies =>  {seriesCurrencies.Count}, [{string.Join(",", seriesCurrencies)}]");

        return false;
    }
}