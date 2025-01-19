using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Services;

public interface IExchangeImporter
{
    bool Scan();
}

public sealed class ExchangeItem
{
    [Name(@"MIC")] public required string Mic { get; init; }

    [Name(@"OPERATING MIC")] public required string OperatingMic { get; init; }

    [Name(@"OPRT/SGMT")] public required string OprtSgmt { get; init; }

    [Name(@"MARKET NAME-INSTITUTION DESCRIPTION")]
    public required string MarketNameInstitutionDescription { get; init; }

    [Name(@"LEGAL ENTITY NAME")] public required string LegalEntityName { get; init; }

    [Name(@"LEI")] public required string Lei { get; init; }

    [Name(@"MARKET CATEGORY CODE")] public required string MarketCategoryCode { get; init; }

    [Name(@"ACRONYM")] public required string Acronym { get; init; }

    [Name(@"ISO COUNTRY CODE (ISO 3166)")] public required string CountryCode { get; init; }

    [Name(@"CITY")] public required string City { get; init; }

    [Name(@"WEBSITE")] public required string WebSite { get; init; }

    [Name(@"STATUS")] public required string Status { get; init; }

    [Name(@"CREATION DATE")]
    [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DateConverter))]
    public DateTime? CreationDate { get; init; }

    [Name(@"LAST UPDATE DATE")]
    [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DateConverter))]
    public DateTime? LastUpdateDate { get; init; }

    [Name(@"LAST VALIDATION DATE")]
    [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DateConverter))]
    public DateTime? LastValidationDate { get; init; }

    [Name(@"EXPIRY DATE")]
    [CsvHelper.Configuration.Attributes.TypeConverter(typeof(DateConverter))]
    public DateTime? ExpiryDate { get; init; }

    [Name(@"COMMENTS")] public required string Comments { get; init; }
}

internal sealed class DateConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }
        
        return DateTime.ParseExact(text, "yyyyMMdd", CultureInfo.InvariantCulture);
    }
}

public sealed class ExchangeImporter : IExchangeImporter
{
    private readonly PortfolioContext m_context;
    private readonly IWebHostEnvironment m_webHostEnvironment;

    private List<ExchangeItem> m_exchanges = new();

    public ExchangeImporter(
        PortfolioContext context,
        IWebHostEnvironment webHostEnvironment
    )
    {
        m_context = context;
        m_webHostEnvironment = webHostEnvironment;
    }

    public bool Scan()
    {
        try
        {
            var filePath = Path.Combine(m_webHostEnvironment.ContentRootPath, "Resources", @"ISO10383_MIC.csv");

            using var reader = new StreamReader(filePath);
            using var cvs = new CsvReader(reader, CultureInfo.InvariantCulture);

            m_exchanges = cvs.GetRecords<ExchangeItem>().ToList();

            var stringProperties = typeof(ExchangeItem).GetProperties()
                .Where(prop => prop.PropertyType == typeof(string)).ToList();

            if (!stringProperties.Any())
            {
                return false;
            }

            // var dic = new ConcurrentDictionary<string, int>();
            //
            // foreach (var exchange in m_exchanges)
            // {
            //     foreach (var prop in stringProperties)
            //     {
            //         var len = prop.GetValue(exchange)?.ToString()?.Trim().Length ?? -1;
            //
            //         dic.AddOrUpdate(prop.Name, (key) => len, (key, prevLen) => Math.Max(prevLen, len));
            //     }
            // }
            
            var entities = m_exchanges.Select(it => new Exchange
            {
                Mic = it.Mic,
                OperatingMic = it.OperatingMic,
                OprtSgmt = it.OprtSgmt,
                MarketNameInstitutionDescription = it.MarketNameInstitutionDescription,
                LegalEntityName = it.LegalEntityName,
                Lei = it.Lei,
                MarketCategoryCode = it.MarketCategoryCode,
                Acronym = it.Acronym,
                CountryCode = it.CountryCode,
                City = it.City,
                WebSite = it.WebSite,
                Status = it.Status,
                CreationDate = it.CreationDate?.ToUniversalTime(),
                LastUpdateDate = it.LastUpdateDate?.ToUniversalTime(),
                LastValidationDate = it.LastValidationDate?.ToUniversalTime(),
                ExpiryDate = it.ExpiryDate?.ToUniversalTime(),
                Comments = it.Comments,
            });
            
            m_context.Exchanges.AddRange(entities);
            
            var count = m_context.SaveChanges();

            return count > 0;
        }
        catch
        {
            return false;
        }
    }
}