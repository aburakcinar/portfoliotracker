namespace FinanceData.Business.Utils;


public static class Undefined
{
    public static decimal Decimal => -999.25M;

    public static double Double => -999.25D;

    public static bool IsUndefined(decimal value)
    {
        return decimal.Equals(value, Undefined.Decimal);
    }
}