using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PortfolioTracker.Data.Migrations.Services;

public interface IExchangeReader
{
    IEnumerable<ExchangeItem> List();
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
    [TypeConverter(typeof(DateConverter))]
    public DateTime? CreationDate { get; init; }

    [Name(@"LAST UPDATE DATE")]
    [TypeConverter(typeof(DateConverter))]
    public DateTime? LastUpdateDate { get; init; }

    [Name(@"LAST VALIDATION DATE")]
    [TypeConverter(typeof(DateConverter))]
    public DateTime? LastValidationDate { get; init; }

    [Name(@"EXPIRY DATE")]
    [TypeConverter(typeof(DateConverter))]
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

public sealed class CvsExchangeReader : IExchangeReader
{
    private List<ExchangeItem> m_exchanges = new();

    public IEnumerable<ExchangeItem> List()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $@"{typeof(ApiDbInitializer).Namespace}.Resources.ISO10383_MIC.csv";

        using var stream = assembly.GetManifestResourceStream(resourceName);

        if(stream == null)
        {
            return Enumerable.Empty<ExchangeItem>();
        }

        using var reader = new StreamReader(stream);
        using var cvs = new CsvReader(reader, CultureInfo.InvariantCulture);

        return cvs.GetRecords<ExchangeItem>().ToList();
    }
}