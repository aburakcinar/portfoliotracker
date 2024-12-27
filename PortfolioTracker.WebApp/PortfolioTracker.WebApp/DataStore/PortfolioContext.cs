using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PortfolioTracker.WebApp.DataStore;

public class PortfolioContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionGroup> TransactionGroups { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }

    public PortfolioContext(DbContextOptions options) : base(options) 
    {
    }
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Currency>().HasData(
            new List<Currency>
            {
                new Currency{ Code = @"TRY", Name = @"Turk Lirasi", Symbol = @"₺"},
                new Currency{ Code = @"USD", Name = @"United States Dollar", Symbol = @"$"},
                new Currency{ Code = @"EUR", Name = @"Euro", Symbol = @"€"},
            }
        );

        modelBuilder.Entity<Stock>().HasData(
            new List<Stock>
            {
                new Stock
                {
                    Id = new Guid(@"{F99512B6-324D-4E13-B75A-19E9301F1565}"),
                    StockExchange = @"XIST",
                    Symbol = @"TUPRS",
                    Name = @"TÜPRAŞ-Türkiye Petrol Rafinerileri"
                },
                new Stock
                {
                    Id = new Guid(@"{1CC7E8B4-F4F0-4CF5-BDB7-A9947F98C55A}"),
                    StockExchange = @"XIST",
                    Symbol = @"DOAS",
                    Name = @"Doğuş Otomotiv"
                },
                new Stock
                {
                    Id = new Guid(@"{a242dc19-adc4-4c8f-a827-416fd8a391b4}"),
                    StockExchange = @"XIST",
                    Symbol = @"ENJSA",
                    Name = @"Enerjisa"
                },
                new Stock
                {
                    Id = new Guid(@"{44829b63-9b2c-4153-ad4d-b194aa5625b6}"),
                    StockExchange = @"XIST",
                    Symbol = @"EREGL",
                    Name = @"Ereğli Demir ve Çelik Fabrikaları"
                },
                new Stock()
                {
                    Id = new Guid(@"{5163feb7-5ceb-4d65-8bfb-64bcad4f370e}"),
                    StockExchange = @"XIST",
                    Symbol = @"FROTO",
                    Name = @"Ford Otosan"
                },
                new Stock()
                {
                    Id = new Guid(@"{b37a0f69-d398-4cec-a460-2835a06ba6bc}"),
                    StockExchange = @"XIST",
                    Symbol = @"GARAN",
                    Name = @"Türkiye Garanti Bankası"
                },
                new Stock
                {
                    Id = new Guid(@"{64eaa2bd-80ab-46a8-9eb3-3112ac1e73b1}"),
                    StockExchange = @"XIST",
                    Symbol = @"ISMEN",
                    Name = @"İş Yatırım Menkul Değerler"
                },
                new Stock
                {
                    Id = new Guid(@"{7f28b900-27eb-4f4a-856a-1e1618ee59ac}"),
                    StockExchange = @"XIST",
                    Symbol = @"THYAO",
                    Name = @"Türk Hava Yolları"
                },
                new Stock
                {
                    Id = new Guid(@"{f7c5e6c8-78a5-4325-ba93-e5f451483866}"),
                    StockExchange = @"XIST",
                    Symbol = @"TKFEN",
                    Name = @"Tekfen Holding"
                },
                new Stock
                {
                    Id = new Guid(@"{b2aa0d1f-f511-4940-a118-38dc6b93b5d1}"),
                    StockExchange = @"XIST",
                    Symbol = @"VESBE",
                    Name = @"Vestel Beyaz Eşya"
                },
                new Stock
                {
                    Id = new Guid(@"{7a46d600-2cc6-4e45-ab74-876064839746}"),
                    StockExchange = @"XIST",
                    Symbol = @"TTRAK",
                    Name = @"Türk Traktör ve Ziraat Makineleri"
                }
                
            }
        );
    }
}

public enum InOut
{
    In = 1,
    Out = 2,
}

public enum TransactionType
{
    Investment = 1,
    InterestCommission = 2,
    Dividend = 3
}

public class Transaction
{
    public Guid Id { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Quantity { get; set; }
    
    public DateTime Created { get; set; }
    
    public InOut InOut { get; set; }

    public TransactionType Type { get; set; }
    
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}

public class TransactionGroup
{
    public Guid Id { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new();
}

public class Stock
{
    public Guid Id { get; set; }
    
    [MaxLength(10)]
    public string StockExchange { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Name { get; set; }= string.Empty;
    
    [MaxLength(255)]
    public string Description { get; set; }= string.Empty;
    
    [MaxLength(15)]
    public string Symbol { get; set; } = string.Empty;
}

public class Holding
{
    public Guid Id { get; set; }
    
    public Guid StockId { get; set; }
    
    [ForeignKey(@"StockId")]
    public required Stock Stock { get; set; }
    
    public Guid TransactionGroupId { get; set; }
    
    [ForeignKey(@"TransactionGroupId")]
    public required TransactionGroup TransactionGroup { get; set; }
}

public class Currency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(10)]
    public string Symbol { get; set; } = string.Empty;
}

public class Portfolio
{
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(10)] public string CurrencyCode { get; set; } = string.Empty;
    
    [ForeignKey(@"CurrencyCode")]
    public Currency? Currency { get; set; }
    
    public List<Holding> Holdings { get; set; } = new();
}