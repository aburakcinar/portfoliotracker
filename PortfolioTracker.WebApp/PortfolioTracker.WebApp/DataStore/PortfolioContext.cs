using Microsoft.EntityFrameworkCore;

namespace PortfolioTracker.WebApp.DataStore;

public class PortfolioContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionGroup> TransactionGroups { get; set; }
    public DbSet<StockPurchase> StockPurchases { get; set; }

    public PortfolioContext(DbContextOptions options) : base(options) 
    {
    }
}

public enum InOut
{
    In = 1,
    Out = 2,
}

public class Transaction
{
    public Guid Id { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public DateTime Created { get; set; }
    
    public InOut InOut { get; set; }

    public string Type { get; set; } = string.Empty;
    
    public string Comment { get; set; } = string.Empty;
}

public class TransactionGroup
{
    public Guid Id { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new();
}

public class StockPurchase
{
    public Guid Id { get; set; }
    
    public string Symbol { get; set; } = string.Empty;
    
    public Guid TransactionGroupId { get; set; }
    
    public required TransactionGroup TransactionGroup { get; set; }
}