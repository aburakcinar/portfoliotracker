using System.Text.Json;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Services;

public sealed class BankAccountItem
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }

    public required string BankName { get; set; }

    public required string AccountHolder { get; set; }

    public required string Description { get; set; }

    public required string Iban { get; set; }

    public required string CurrencyCode { get; set; }
    
    public required string LocaleCode { get; set; }

    public DateTime OpenDate { get; set; }
}

public sealed class BankAccountDepositItem
{
    public Guid BankAccountId { get; set; }
    
    public InOut InOut { get; set; }
    
    public decimal Amount { get; set; }
    
    public decimal Expenses { get; set; }
    
    public decimal Taxes { get; set; }
    
    public DateTime Created { get; set; }
}

public interface IPortfolioImportService
{
    Task<IEnumerable<BankAccountItem>> ListBankAccountsAsync();

    Task<IEnumerable<BankAccountDepositItem>> ListDepositsAsync();
}

public class PortfolioImportService : IPortfolioImportService
{
    private readonly IConfiguration m_configurationManager;

    public PortfolioImportService(IConfiguration configurationManager)
    {
        m_configurationManager = configurationManager;
    }

    public async Task<IEnumerable<BankAccountItem>> ListBankAccountsAsync()
    {
        var migrationPath = m_configurationManager.GetValue<string>(@"MigrationDataFolder");
        
        var filePath = Path.Combine(migrationPath!, @"BankAccounts.database.json");
        
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var result = await JsonSerializer.DeserializeAsync<List<BankAccountItem>>(stream,options);

        return result!;
    }

    public async Task<IEnumerable<BankAccountDepositItem>> ListDepositsAsync()
    {
        var migrationPath = m_configurationManager.GetValue<string>(@"MigrationDataFolder");
        
        var filePath = Path.Combine(migrationPath!, @"BankAccountDeposits.database.json");
        
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var result = await JsonSerializer.DeserializeAsync<List<BankAccountDepositItem>>(stream,options);
        
        return result!;
    }
}