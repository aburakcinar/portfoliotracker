namespace PortfolioTracker.Data.Models;

public class BankAccountTransactionGroupModel
{
    public Guid Id { get; set; }

    public Guid BankAccountId { get; set; }

    public string ReferenceNo { get; set; } = string.Empty;

    public List<BankAccountTransactionModel> Transactions { get; set; } = new();

    public string Description { get; set; } = string.Empty;

    public string ActionTypeCode { get; set; } = string.Empty;

    public string ActionTypeName { get; set; } = string.Empty;

    public DateTime ExecuteDate { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public bool IsDraft { get; set; } = true;
}


public class BankAccountTransactionModel
{
    public Guid Id { get; set; }

    public Guid BankAccountTransactionGroupId { get; set; }

    public decimal Price { get; set; }

    public decimal Quantity { get; set; }

    public InOut InOut { get; set; }

    public TransactionType TransactionType { get; set; }

    public string? Description { get; set; } = null!;
}
