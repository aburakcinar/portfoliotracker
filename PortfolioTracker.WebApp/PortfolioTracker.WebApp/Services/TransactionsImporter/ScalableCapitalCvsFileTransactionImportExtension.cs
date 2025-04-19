using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Services.TransactionsImporter;

public class ScalableCapitalCvsFileTransactionImportExtension : ITransactionImportExtension
{
    private readonly IPortfolioContext m_context;

    public ScalableCapitalCvsFileTransactionImportExtension(IPortfolioContext context)
    {
        m_context = context;
    }

    public string ImportSourceType => ImportSourceTypes.ScalableCapitalCvs;

    public async Task<List<TransactionImportItem>> ImportAsync(Stream dataStream)
    {
        var config = new CsvConfiguration(CultureInfo.GetCultureInfo("nl-NL"))
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            HasHeaderRecord = true,
            Delimiter = @";"
        };
        
        using var reader = new StreamReader(dataStream);
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<Transaction>();
        
        var lst = new List<TransactionImportItem>();

        foreach (var record in records)
        {
            var executeDate = record.Date.Date.Add(record.Time.TimeOfDay);
            
            var transactionType = GetTransactionType(record);

            if (string.IsNullOrEmpty(transactionType))
            {
                continue;
            }
            
            lst.Add(new TransactionImportItem
            {
                ExecuteDate = executeDate,
                ReferenceNo = record.Reference,
                TransactionType = transactionType,
                Description = record.Description,
                Asset = GetAsset(record),
                Quantity = record.Shares ?? 0,
                Price = record.Price ?? 0,
                Amount = record.Amount ?? 0,
                Fee = record.Fee ?? 0,
                Tax = record.Tax ?? 0,
            });
        }

        await Task.CompletedTask;

        return lst;
    }

    private Asset? GetAsset(Transaction record)
    {
        return string.IsNullOrEmpty(record.Isin) ? null : m_context.Assets.FirstOrDefault(x => x.Isin == record.Isin);
    }

    private string GetTransactionType(Transaction record)
    {
        var assetType = record.AssetType.ToLower();
        var type = record.Type.ToLower();

        if (assetType == @"cash")
        {
            if (type == @"deposit")
            {
                return TransactionActionTypes.DEPOSIT;
            }
            else if (type == @"credit" || type == @"withdraw")
            {
                return TransactionActionTypes.WITHDRAW;
            }
            else if (type == @"distribution")
            {
                return TransactionActionTypes.DIVIDEND_DISTRIBUTION;
            }
            else if (type == @"fee")
            {
                return TransactionActionTypes.ACCOUNT_FEE;
            }
            else if (type == @"interest")
            {
                return TransactionActionTypes.INTEREST;
            }
        }
        else if (assetType == @"security")
        {
            if (type == @"buy" || type == @"saving plan")
            {
                return TransactionActionTypes.BUY_ASSET;
            }
            else if (type == @"sell")
            {
                return TransactionActionTypes.SELL_ASSET;
            }
        }
        
        Debug.WriteLine($@"Asset Type: {assetType}, Type: {type}");
        
        return string.Empty;
    }
}


public class Transaction
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string Status { get; set; } = null!;
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = null!;
    public string AssetType { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Isin { get; set; } = string.Empty;
    public decimal? Shares { get; set; }
    public decimal? Price { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Fee { get; set; }
    public decimal? Tax { get; set; }
    public string Currency { get; set; } = string.Empty;
}
