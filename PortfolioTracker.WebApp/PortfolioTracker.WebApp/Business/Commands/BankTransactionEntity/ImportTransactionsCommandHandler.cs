using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Business.Commands.BankTransactionEntity;

public sealed class ImportTransactionsCommand : IRequest<bool>
{
    
}

public sealed class ImportTransactionsCommandHandler : IRequestHandler<ImportTransactionsCommand, bool>
{
    private readonly IPortfolioContext m_context;
    private readonly IPortfolioImportService m_portfolioImportService;

    public ImportTransactionsCommandHandler(
        IPortfolioContext context, 
        IPortfolioImportService portfolioImportService
        )
    {
        m_context = context;
        m_portfolioImportService = portfolioImportService;
    }

    public async Task<bool> Handle(ImportTransactionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var transactions = await m_portfolioImportService.ListDepositsAsync();
            var transactionActionTypes = await m_context.TransactionActionTypes.ToListAsync(cancellationToken);
            
            var transactionsByBankAccount = transactions.GroupBy(x => x.BankAccountId);
            
            foreach (var itemsByBankAccount in transactionsByBankAccount)
            {
                var bankAccountId = itemsByBankAccount.Key;
            
                var bankAccount = await m_context
                    .BankAccounts
                    .Include(x => x.TransactionGroups)
                    .ThenInclude(transactionGroup => transactionGroup.Transactions)
                    .FirstOrDefaultAsync(x => x.Id == bankAccountId, cancellationToken);
            
                if (bankAccount is  null)
                {
                    continue;
                }
            
                foreach (var deposit in itemsByBankAccount.Select(x => x))
                {
                    var actionTypeCode = deposit.InOut == InOut.Incoming ? "DEPOSIT" : "WITHDRAW";
                    var transactionActionType = transactionActionTypes.First(x => x.Code == actionTypeCode);

                    var group = new BankAccountTransactionGroup
                    {
                        Id = Guid.NewGuid(),
                        BankAccountId = bankAccount.Id,
                        Description = $@"{transactionActionType.Name}"
                    };
                    
                    // Main Action
                    m_context.BankAccountTransactions.Add(new BankAccountTransaction
                    {
                        Id = Guid.NewGuid(),
                        BankAccountTransactionGroupId = group.Id,
                        InOut = deposit.InOut,
                        Price = deposit.Amount,
                        Quantity = 1M,
                        ActionTypeCode = actionTypeCode,
                        ActionType = transactionActionType,
                        Created = deposit.Created,
                        Description = String.Empty,
                    });

                    if (deposit.Expenses > 0)
                    {
                        m_context.BankAccountTransactions.Add(new BankAccountTransaction
                        {
                            Id = Guid.NewGuid(),
                            BankAccountTransactionGroupId = group.Id,
                            InOut = InOut.Outgoing,
                            Price = deposit.Expenses,
                            Quantity = 1M,
                            ActionTypeCode = @$"{actionTypeCode}_FEE",
                            ActionType = transactionActionTypes.First(x => x.Code == @$"{actionTypeCode}_FEE"),
                            Created = deposit.Created,
                            Description = String.Empty,
                        });
                    }
                    
                    if (deposit.Taxes > 0)
                    {
                        m_context.BankAccountTransactions.Add(new BankAccountTransaction
                        {
                            Id = Guid.NewGuid(),
                            BankAccountTransactionGroupId = group.Id,
                            InOut = InOut.Outgoing,
                            Price = deposit.Taxes,
                            Quantity = 1M,
                            ActionTypeCode = @$"{actionTypeCode}_TAX",
                            ActionType = transactionActionTypes.First(x => x.Code == @$"{actionTypeCode}_TAX"),
                            Created = deposit.Created,
                            Description = String.Empty,
                        });
                    }
                    
                    m_context.BankAccountTransactionGroups.Add(group);
                }
            }

            var result = await m_context.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
        catch
        {
            return false;
        }
    }
}