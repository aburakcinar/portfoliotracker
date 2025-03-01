using System.Globalization;
using System.Xml;
using FinanceData.Business.Services;
using FinanceData.Business.Utils;

namespace FinanceData.Business.Extensions;

public static class XmlExtensions
{
    public static decimal GetAsDecimal(this XmlNode? node, string field)
    {
        if (node is not { Attributes: { } })
        {
            return Undefined.Decimal;
        }

        if (decimal.TryParse(node.Attributes[field]?.Value ?? string.Empty, NumberStyles.Currency,
                CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        return Undefined.Decimal;
    }

    public static string Get(this XmlNode? node, string field)
    {
        if (node is not { Attributes: { } })
        {
            return string.Empty;
        }

        return node.Attributes[field]?.Value ?? string.Empty;
    }

    public static DateOnly? GetDateOnly(this XmlNode? node, string field)
    {
        var result = node.GetDateTime(field);

        if (result.HasValue)
        {
            return DateOnly.FromDateTime(result.Value);
        }

        return null;
    }

    public static DateTime? GetDateTime(this XmlNode? node, string field)
    {
        if (node is not { Attributes: { } })
        {
            return null;
        }

        string value = node.Attributes[field]?.Value ?? string.Empty;

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite,
                out DateTime dateTime))
        {
            return dateTime;
        }

        return null;
    }
}