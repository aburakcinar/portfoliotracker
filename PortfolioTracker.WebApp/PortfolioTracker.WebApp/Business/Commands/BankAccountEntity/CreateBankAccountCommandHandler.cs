using MediatR;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Commands.BankAccountEntity;

public sealed class CreateBankAccountCommand : IRequest<bool>
{
    public required string Name { get; init; }

    public required string BankName { get; init; }

    public required string AccountHolder { get; init; }

    public required string Description { get; init; }

    public required string Iban { get; init; }

    public required string CurrencyCode { get; init; }
    
    public required string LocaleCode { get; init; }

    public DateTime OpenDate { get; init; }

}

public sealed class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, bool>
{
    private readonly IPortfolioContext m_context;

    public CreateBankAccountCommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = new BankAccount
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                BankName = request.BankName,
                AccountHolder = request.AccountHolder,
                Description = request.Description,
                Iban = request.Iban,
                CurrencyCode = request.CurrencyCode,
                LocaleCode = request.LocaleCode,
                OpenDate = request.OpenDate,
                Created = DateTime.Now.ToUniversalTime()
            };
            
            m_context.BankAccounts.Add(item);
            
            await m_context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}