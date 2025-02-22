using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Business.Commands.BankAccountEntity;

public sealed class ImportBankAccountCommand : IRequest<bool>
{
}

public sealed class ImportBankAccountCommandHandler : IRequestHandler<ImportBankAccountCommand, bool>
{
    private readonly PortfolioContext m_context;
    private readonly IPortfolioImportService m_portfolioImportService;

    public ImportBankAccountCommandHandler(
        PortfolioContext context, 
        IPortfolioImportService portfolioImportService
        )
    {
        m_context = context;
        m_portfolioImportService = portfolioImportService;
    }

    public async Task<bool> Handle(ImportBankAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingBankAccounts = await m_context.BankAccounts.ToListAsync(cancellationToken);
            var bankAccountsToImport = await m_portfolioImportService.ListBankAccountsAsync();

            foreach (var bankAccount in bankAccountsToImport)
            {
                var found = existingBankAccounts.FirstOrDefault(x => x.Id == bankAccount.Id);

                if (found is null)
                {
                    m_context.BankAccounts.Add(new BankAccount
                    {
                        Id = bankAccount.Id,
                        Name = bankAccount.Name,
                        BankName = bankAccount.BankName,
                        AccountHolder = bankAccount.AccountHolder,
                        Description = bankAccount.Description,
                        Iban = bankAccount.Iban,
                        CurrencyCode = bankAccount.CurrencyCode,
                        LocaleCode = bankAccount.LocaleCode,
                        OpenDate = bankAccount.OpenDate,
                        Created = DateTime.Now.ToUniversalTime(),
                    });
                }
            }
            
            var result = await m_context.SaveChangesAsync(cancellationToken);
            
            return result > 0;
        }
        catch
        {
            return false;
        }
    }
}