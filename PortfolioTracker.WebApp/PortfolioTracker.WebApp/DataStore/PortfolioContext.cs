using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.Tools;

namespace PortfolioTracker.WebApp.DataStore;

public class PortfolioContext : DbContext
{
    #region V1

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionGroup> TransactionGroups { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Locale> Locales { get; set; }
    public DbSet<Exchange> Exchanges { get; set; }

    #endregion

    #region V2

    public DbSet<PortfolioV2> PortfolioV2s { get; set; }

    public DbSet<HoldingV2> HoldingV2s { get; set; }

    public DbSet<Asset> Assets { get; set; }
    
    public DbSet<BankAccount> BankAccounts { get; set; }
    
    public DbSet<BankAccountTransactionGroup> BankAccountTransactionGroups { get; set; }
    
    public DbSet<BankAccountTransaction> BankAccountTransactions { get; set; }
    
    public DbSet<TransactionActionType> TransactionActionTypes { get; set; }

    #endregion

    public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
                },
                new Stock
                {
                    Id = new Guid(@"{4A4EE229-7D3E-4490-850B-3F467862FF15}"),
                    StockExchange = @"XIST",
                    Symbol = @"BASGZ",
                    Name = @"Baskent Dogalgaz Dagitim Gayr Yat OrtAS"
                },
                new Stock
                {
                    Id = new Guid(@"{EE0111BD-BB3E-42F8-9E31-42CF06D71EFA}"),
                    StockExchange = @"XIST",
                    Symbol = @"INDES",
                    Name = @"Indeks Blgsyr Sstmlr Mhndslk Sny v Tcrt"
                }
            }
        );

        modelBuilder
            .Entity<PortfolioV2>()
            .HasData(new List<PortfolioV2>()
            {
                new PortfolioV2
                {
                    Id = new Guid(@"{25D4CFE9-076D-48E1-A04B-A0B139BB8864}"),
                    Name = @"Default",
                    Description = @"Default Portfolio",
                    IsDefault = true,
                    Created = DateTime.Now.ToUniversalTime(),
                }
            });

        modelBuilder
            .Entity<TransactionActionType>()
            .HasData(TransactionActionTypeTool.ScanFromConstants());
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

    [MaxLength(255)] public string Description { get; set; } = string.Empty;

    public Guid TransactionGroupId { get; set; }

    [ForeignKey(@"TransactionGroupId")] public TransactionGroup TransactionGroup { get; set; } = new();
}

public class TransactionGroup
{
    public Guid Id { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
}

public class Stock
{
    public Guid Id { get; set; }

    [MaxLength(10)] public string StockExchange { get; set; } = string.Empty;

    [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(255)] public string Description { get; set; } = string.Empty;

    [MaxLength(15)] public string Symbol { get; set; } = string.Empty;
}

public class Holding
{
    public Guid Id { get; set; }

    public Guid PortfolioId { get; set; }

    [ForeignKey(@"PortfolioId")] public required Portfolio Portfolio { get; set; }

    public Guid StockId { get; set; }

    [ForeignKey(@"StockId")] public required Stock Stock { get; set; }

    public Guid TransactionGroupId { get; set; }

    [ForeignKey(@"TransactionGroupId")] public required TransactionGroup TransactionGroup { get; set; }
}

public class Currency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(100)] public required string Name { get; set; }

    [MaxLength(100)] public required string NameLocal { get; set; }

    [MaxLength(10)] public required string Symbol { get; set; }

    public int SubunitValue { get; set; }

    [MaxLength(20)] public required string SubunitName { get; set; }
}

public class Portfolio
{
    public Guid Id { get; set; }

    [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(255)] public string Description { get; set; } = string.Empty;

    [MaxLength(10)] public string CurrencyCode { get; set; } = string.Empty;

    [ForeignKey(@"CurrencyCode")] public Currency? Currency { get; set; }

    public List<Holding> Holdings { get; set; } = new();
}

public class Locale
{
    [Key] [MaxLength(6)] public required string LocaleCode { get; set; }

    [MaxLength(25)] public required string LanguageName { get; set; }

    [MaxLength(25)] public required string LanguageNameLocal { get; set; }

    [MaxLength(50)] public required string CountryName { get; set; }

    [MaxLength(50)] public required string CountryNameLocal { get; set; }

    [MaxLength(2)] public required string CountryCode { get; set; }

    [MaxLength(30)] public required string CurrencyName { get; set; }

    [MaxLength(100)] public required string CurrencyNameLocal { get; set; }

    [MaxLength(3)] public required string CurrencyCode { get; set; }

    [MaxLength(6)] public required string CurrencySymbol { get; set; }

    public int CurrencySubunitValue { get; set; }

    [MaxLength(20)] public required string CurrencySubunitName { get; set; }
}

public class Exchange
{
    [Key] [MaxLength(4)] public required string Mic { get; set; }

    [MaxLength(4)] public required string OperatingMic { get; set; }

    [MaxLength(4)] public required string OprtSgmt { get; set; }

    [MaxLength(255)] public required string MarketNameInstitutionDescription { get; set; }

    [MaxLength(255)] public required string LegalEntityName { get; set; }

    [MaxLength(20)] public required string Lei { get; set; }

    [MaxLength(4)] public required string MarketCategoryCode { get; set; }

    [MaxLength(255)] public required string Acronym { get; set; }

    [MaxLength(2)] public required string CountryCode { get; set; }

    [MaxLength(64)] public required string City { get; set; }

    [MaxLength(255)] public required string WebSite { get; set; }

    [MaxLength(7)] public required string Status { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public DateTime? LastValidationDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [MaxLength(255)] public required string Comments { get; set; }
}

public class StockItem
{
    [Key] [MaxLength(100)] public required string FullCode { get; set; }

    [MaxLength(4)] public required string StockExchangeCode { get; set; }

    [ForeignKey(@"StockExchangeCode")] public required Exchange StockExchange { get; set; }

    [MaxLength(10)] public required string Symbol { get; set; }

    [MaxLength(255)] public required string Name { get; set; }

    [MaxLength(2000)] public required string Description { get; set; }

    [MaxLength(6)] public required string LocaleCode { get; set; }

    [ForeignKey(@"LocaleCode")] public required Locale Locale { get; set; }

    [MaxLength(255)] public required string WebSite { get; set; }
}

// New Versions

public class PortfolioV2
{
    [Key] public Guid Id { get; set; }

    [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(255)] public string Description { get; set; } = string.Empty;

    public List<HoldingV2> Holdings { get; set; } = new();

    public bool IsDefault { get; set; }

    public DateTime Created { get; set; }
}

public class HoldingV2
{
    [Key] public Guid Id { get; set; }

    public Guid PortfolioId { get; set; }

    [ForeignKey(@"PortfolioId")] public required PortfolioV2 Portfolio { get; set; }

    public Guid AssetId { get; set; }

    [ForeignKey(@"AssetId")] public required Asset Asset { get; set; }

    public Guid TransactionGroupId { get; set; }

    [ForeignKey(@"TransactionGroupId")] public required TransactionGroup TransactionGroup { get; set; }

    public DateTime Created { get; set; }
}

public enum AssetTypes
{
    Stock = 1,
    ETF = 2,
    Commodity = 3,
    Bond = 4,
    Crypto = 5,
}

public class Asset
{
    [Key] public Guid Id { get; set; }

    [MaxLength(10)] public string TickerSymbol { get; set; } = string.Empty;

    [MaxLength(4)] public string ExchangeCode { get; set; } = string.Empty;

    [ForeignKey(@"ExchangeCode")] public required Exchange Exchange { get; set; }

    [MaxLength(10)] public string CurrencyCode { get; set; } = string.Empty;

    [ForeignKey(@"CurrencyCode")] public required Currency Currency { get; set; }

    public AssetTypes AssetType { get; set; }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [MaxLength(2000)] public string Description { get; set; } = string.Empty;

    [MaxLength(12)] public string Isin { get; set; } = string.Empty;

    [MaxLength(6)] public string Wkn { get; set; } = string.Empty;

    [MaxLength(255)] public string WebSite { get; set; } = string.Empty;

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    [Column(TypeName = "decimal(18, 2)")] public decimal Price { get; set; }
}

public class BankAccount
{
    [Key] public Guid Id { get; set; }

    [MaxLength(255)] public required string Name { get; set; }

    [MaxLength(255)] public required string BankName { get; set; }

    [MaxLength(255)] public required string AccountHolder { get; set; }

    [MaxLength(2000)] public required string Description { get; set; }

    [MaxLength(100)] public required string Iban { get; set; }

    [MaxLength(10)] public required string CurrencyCode { get; set; }

    [ForeignKey(@"CurrencyCode")] public Currency Currency { get; set; } = null!;

    [MaxLength(6)] public required string LocaleCode { get; set; }

    [ForeignKey(@"LocaleCode")] public Locale Locale { get; set; } = null!;

    public DateTime OpenDate { get; set; }

    public DateTime Created { get; set; }

    public List<BankAccountTransactionGroup> TransactionGroups { get; set; } = new();
}

public class BankAccountTransactionGroup
{
    [Key] public Guid Id { get; set; }

    public Guid BankAccountId { get; set; }

    public List<BankAccountTransaction> Transactions { get; set; } = new();
}

public class BankAccountTransaction
{
    [Key] public Guid Id { get; set; }

    public Guid BankAccountTransactionGroupId { get; set; }

    public decimal Price { get; set; }

    public decimal Quantity { get; set; }

    public DateTime Created { get; set; }

    public InOut InOut { get; set; }

    public TransactionType Type { get; set; }

    [MaxLength(255)] public string Description { get; set; } = string.Empty;
}

public enum TransactionActionTypeCategory
{
    Unknown = 0,
    Payment = 1,
    Tax = 2,
    Fee = 3
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class TransactionActionTypes
{
    #region Deposit 
    
    [Description(@"Deposit to target account")]
    [Category(nameof(TransactionActionTypeCategory.Payment))]
    public const string DEPOSIT = @"DEPOSIT";
    
    [Description(@"Deposit fee on target account")]
    [Category(nameof(TransactionActionTypeCategory.Fee))]
    public const string DEPOSIT_FEE = @"DEPOSIT_FEE";
    
    [Description(@"Tax on Deposit from target account")]
    [Category(nameof(TransactionActionTypeCategory.Tax))]
    public const string DEPOSIT_TAX = @"DEPOSIT_TAX";
    
    #endregion
    
    #region Withdraw
    
    [Description(@"Withdraw from target account")]
    [Category(nameof(TransactionActionTypeCategory.Payment))]
    public const string WITHDRAW = @"WITHDRAW";
    
    [Description(@"Withdraw action fee on target account")]
    [Category(nameof(TransactionActionTypeCategory.Fee))]
    public const string WITHDRAW_FEE = @"WITHDRAW_FEE";
    
    [Description(@"Tax on Withdraw action on target account")]
    [Category(nameof(TransactionActionTypeCategory.Tax))]
    public const string WITHDRAW_TAX = @"WITHDRAW_TAX";
    
    #endregion
    
    #region Payment
    
    [Description(@"Payment from target account")]
    [Category(nameof(TransactionActionTypeCategory.Payment))]
    public const string PAYMENT = @"PAYMENT";
    
    [Description(@"Payment action fee on target account")]
    [Category(nameof(TransactionActionTypeCategory.Fee))]
    public const string PAYMENT_FEE = @"PAYMENT_FEE";
    
    [Description(@"Tax on Payment action on target account")]
    [Category(nameof(TransactionActionTypeCategory.Tax))]
    public const string PAYMENT_TAX = @"PAYMENT_TAX";
    
    #endregion
    
    #region Account Fee
    
    [Description(@"Account usage fee on target account")]
    [Category(nameof(TransactionActionTypeCategory.Payment))]
    public const string ACCOUNT_FEE = @"ACCOUNT_FEE";
    
    #endregion
    
    #region Dividend
    
    [Description(@"Dividend payment to target account")]
    [Category(nameof(TransactionActionTypeCategory.Payment))]
    public const string DIVIDEND_DISTRIBUTION = @"DIVIDEND_DISTRIBUTION";
    
    [Description(@"Dividend distribution fee on target account")]
    [Category(nameof(TransactionActionTypeCategory.Fee))]
    public const string DIVIDEND_DISTRIBUTION_FEE = @"DIVIDEND_DISTRIBUTION_FEE";
    
    [Description(@"Dividend Withholding tax")]
    [Category(nameof(TransactionActionTypeCategory.Tax))]
    public const string DIVIDEND_WITHHOLDING_TAX = @"DIVIDEND_WITHHOLING_TAX";

    #endregion
    
    #region Interest
    
    [Description(@"Interest payment to target account")]
    [Category(nameof(TransactionActionTypeCategory.Payment))]
    public const string INTEREST = @"INTEREST";
    
    [Description(@"Interest fee from target account")]
    [Category(nameof(TransactionActionTypeCategory.Fee))]
    public const string INTEREST_FEE = @"INTEREST_FEE";

    [Description(@"Tax on Interest from target account")]
    [Category(nameof(TransactionActionTypeCategory.Tax))]
    public const string INTEREST_TAX = @"INTEREST_TAX";
    
    #endregion
}

public class TransactionActionType
{
    [Key] [MaxLength(50)] public required string Code { get; set; }
    
    [MaxLength(255)] public required string Description { get; set; }
    
    public TransactionActionTypeCategory Category { get; set; }
}