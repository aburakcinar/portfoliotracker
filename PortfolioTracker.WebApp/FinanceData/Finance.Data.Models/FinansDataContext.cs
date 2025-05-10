using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinanceData.Business.DataStore;

public interface IFinansDataContext
{
    DbSet<CurrencyExchangeRatio> CurrencyExchangeRatios { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class FinansDataContext : DbContext, IFinansDataContext
{
    public DbSet<CurrencyExchangeRatio> CurrencyExchangeRatios { get; set; } = default!;

    public FinansDataContext(DbContextOptions<FinansDataContext> options) : base(options)
    {
    }
}

public class CurrencyExchangeRatio
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    [MaxLength(10)] public required string BaseCurrency { get; set; }

    [MaxLength(10)] public required string TargetCurrency { get; set; }

    public decimal Rate { get; set; }
}

public static class FinansDataContextExtensions
{
    public static string AsKey(this CurrencyExchangeRatio item)
    {
        return $@"{item.Date.ToString("yyyyMMdd")}_{item.BaseCurrency}_{item.TargetCurrency}";
    }
}