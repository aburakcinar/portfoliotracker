using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PortfolioTracker.WebApp.DataStore;

public class PortfolioContext : DbContext
{
    // Definitions
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Locale> Locales { get; set; }
    public DbSet<Exchange> Exchanges { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<TransactionActionType> TransactionActionTypes { get; set; }
    public DbSet<CurrencyExchangeRates> CurrencyExchangeRates { get; set; }

    // UserData
    public DbSet<BankAccount> BankAccounts { get; set; }

    public DbSet<BankAccountTransactionGroup> BankAccountTransactionGroups { get; set; }

    public DbSet<BankAccountTransaction> BankAccountTransactions { get; set; }


    public DbSet<Portfolio> Portfolios { get; set; }

    public DbSet<Holding> Holdings { get; set; }
    

    public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
    {
    }
}

public enum InOut
{
    Incoming = 1,
    Outgoing = 2,
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

public class Portfolio
{
    [Key] public Guid Id { get; set; }

    [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(255)] public string Description { get; set; } = string.Empty;

    public List<Holding> Holdings { get; set; } = new();

    public bool IsDefault { get; set; }

    public DateTime Created { get; set; }

    public Guid BankAccountId { get; set; }

    [ForeignKey(@"BankAccountId")] public BankAccount BankAccount { get; set; } = null!;
}

public class Holding
{
    [Key] public Guid Id { get; set; }

    public Guid PortfolioId { get; set; }

    [ForeignKey(@"PortfolioId")] public required Portfolio Portfolio { get; set; }

    public Guid AssetId { get; set; }

    [ForeignKey(@"AssetId")] public required Asset Asset { get; set; }

    public Guid BankAccountTransactionGroupId { get; set; }

    [ForeignKey(@"BankAccountTransactionGroupId")] public required BankAccountTransactionGroup BankAccountTransactionGroup { get; set; }

    public DateOnly ExecuteDate { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime? Updated { get; set; }
}

public enum AssetTypes
{
    [Display(Name = "Stocks")]
    Stock = 1,
    [Display(Name = "ETFs")]
    ETF = 2,
    [Display(Name = "Commodities")]
    Commodity = 3,
    [Display(Name = "Bonds")]
    Bond = 4,
    [Display(Name = "Cryptos")]
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
    
    [MaxLength(255)] public string Description { get; set; } = string.Empty;
    
    public DateOnly ExecuteDate { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime? Updated { get; set; }
}

public class BankAccountTransaction
{
    [Key] public Guid Id { get; set; }

    public Guid BankAccountTransactionGroupId { get; set; }

    public decimal Price { get; set; }

    public decimal Quantity { get; set; }

    public DateTime Created { get; set; }
    
    public DateTime? Updated { get; set; }

    public InOut InOut { get; set; }

    [MaxLength(50)] public string ActionTypeCode { get; set; } = string.Empty;

    [ForeignKey(@"ActionTypeCode")] public TransactionActionType ActionType { get; set; } = null!;

    [MaxLength(255)] public string Description { get; set; } = string.Empty;
}

public enum TransactionActionTypeCategory
{
    Unknown = 0,
    Outgoing = 1,
    Tax = 2,
    Fee = 3,
    Incoming = 4
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class TransactionActionTypes
{
    #region Deposit

    [Display(Name = "Deposit", Description = "Deposit to target account", GroupName = nameof(TransactionActionTypeCategory.Incoming))]
    public static readonly string DEPOSIT = "DEPOSIT";

    [Display(Name = "Deposit Fee", Description = "Deposit fee on target account", GroupName = nameof(TransactionActionTypeCategory.Fee))]
    public static readonly string DEPOSIT_FEE = "DEPOSIT_FEE";

    [Display(Name = "Deposit Tax", Description = "Tax on Deposit from target account", GroupName = nameof(TransactionActionTypeCategory.Tax))]
    public static readonly string DEPOSIT_TAX = "DEPOSIT_TAX";

    #endregion

    #region Withdraw

    [Display(Name = "Withdraw", Description = "Withdraw from target account", GroupName = nameof(TransactionActionTypeCategory.Outgoing))]
    public static readonly string WITHDRAW = "WITHDRAW";

    [Display(Name = "Withdraw Fee", Description = "Withdraw action fee on target account", GroupName = nameof(TransactionActionTypeCategory.Fee))]
    public static readonly string WITHDRAW_FEE = "WITHDRAW_FEE";

    [Display(Name = "Withdraw Tax", Description = "Tax on Withdraw action on target account", GroupName = nameof(TransactionActionTypeCategory.Tax))]
    public static readonly string WITHDRAW_TAX = "WITHDRAW_TAX";

    #endregion

    #region Payment

    [Display(Name = "Payment", Description = "Payment from target account", GroupName = nameof(TransactionActionTypeCategory.Outgoing))]
    public static readonly string PAYMENT = "PAYMENT";

    [Display(Name = "Payment Fee", Description = "Payment action fee on target account", GroupName = nameof(TransactionActionTypeCategory.Fee))]
    public static readonly string PAYMENT_FEE = "PAYMENT_FEE";

    [Display(Name = "Payment Tax", Description = "Tax on Payment action on target account", GroupName = nameof(TransactionActionTypeCategory.Tax))]
    public static readonly string PAYMENT_TAX = "PAYMENT_TAX";

    #endregion
    
    #region Buy Asset

    [Display(Name = "Buy Asset", Description = "Buy Asset from target account", GroupName = nameof(TransactionActionTypeCategory.Outgoing))]
    public static readonly string BUY_ASSET = "BUY_ASSET";

    [Display(Name = "Buy Asset Fee", Description = "Buy Asset action fee on target account", GroupName = nameof(TransactionActionTypeCategory.Fee))]
    public static readonly string BUY_ASSET_FEE = "BUY_ASSET_FEE";

    [Display(Name = "Buy Asset Tax", Description = "Tax on Buy Asset action on target account", GroupName = nameof(TransactionActionTypeCategory.Tax))]
    public static readonly string BUY_ASSET_TAX = "BUY_ASSET_TAX";

    #endregion
    
    
    #region Sell Asset

    [Display(Name = "Sell Asset", Description = "Sell Asset from target account",
        GroupName = nameof(TransactionActionTypeCategory.Incoming))]
    public static readonly string SELL_ASSET = "SELL_ASSET";

    [Display(Name = "Sell Asset Fee", Description = "Sell Asset action fee on target account", GroupName = nameof(TransactionActionTypeCategory.Fee))]
    public static readonly string SELL_ASSET_FEE = "SELL_ASSET_FEE";

    [Display(Name = "Sell Asset Tax", Description = "Sell on Buy Asset action on target account", GroupName = nameof(TransactionActionTypeCategory.Tax))]
    public static readonly string SELL_ASSET_TAX = "SELL_ASSET_TAX";

    #endregion

    #region Account Fee

    [Display(Name = "Account Fee", Description = "Account usage fee on target account", GroupName = nameof(TransactionActionTypeCategory.Outgoing))]
    public static readonly string ACCOUNT_FEE = "ACCOUNT_FEE";

    #endregion

    #region Dividend

    [Display(Name = "Dividend Distribution", Description = "Dividend payment to target account", GroupName = nameof(TransactionActionTypeCategory.Incoming))]
    public static readonly string DIVIDEND_DISTRIBUTION = "DIVIDEND_DISTRIBUTION";

    [Display(Name = "Dividend Distribution Fee", Description = "Dividend distribution fee on target account", GroupName = nameof(TransactionActionTypeCategory.Fee))]
    public static readonly string DIVIDEND_DISTRIBUTION_FEE = "DIVIDEND_DISTRIBUTION_FEE";

    [Display(Name = "Dividend Withholding Tax", Description = "Dividend Withholding tax", GroupName = nameof(TransactionActionTypeCategory.Tax))]
    public static readonly string DIVIDEND_WITHHOLDING_TAX = "DIVIDEND_WITHHOLDING_TAX";

    #endregion

    #region Interest

    [Display(Name = "Interest", Description = "Interest payment to target account", GroupName = nameof(TransactionActionTypeCategory.Incoming))]
    public static readonly string INTEREST = "INTEREST";

    [Display(Name = "Interest Fee", Description = "Interest fee from target account", GroupName = nameof(TransactionActionTypeCategory.Fee))]
    public static readonly string INTEREST_FEE = "INTEREST_FEE";

    [Display(Name = "Interest Tax", Description = "Tax on Interest from target account", GroupName = nameof(TransactionActionTypeCategory.Tax))]
    public static readonly string INTEREST_TAX = "INTEREST_TAX";

    #endregion
}

public class TransactionActionType
{
    [Key] [MaxLength(50)] public required string Code { get; set; }
    
    [MaxLength(50)] public required string Name { get; set; }

    [MaxLength(255)] public required string Description { get; set; }

    public TransactionActionTypeCategory Category { get; set; }
}

public class CurrencyExchangeRates
{
    [Key] public Guid Id { get; set; }
    
    [MaxLength(10)] public required string FromCurrencyCode { get; set; } 

    [MaxLength(10)] public required string ToCurrencyCode { get; set; } 

    [Column(TypeName = "date")]
    public DateOnly Date { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal Open { get; set; }

    public decimal High { get; set; }

    public decimal Low { get; set; }
}