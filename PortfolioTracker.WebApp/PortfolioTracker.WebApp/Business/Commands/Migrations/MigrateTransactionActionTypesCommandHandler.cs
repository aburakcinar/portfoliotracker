using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Tools;

namespace PortfolioTracker.WebApp.Business.Commands.Migrations;

public sealed class MigrateTransactionActionTypesCommand : IRequest<bool>
{
}

public sealed class MigrateTransactionActionTypesCommandHandler : IRequestHandler<MigrateTransactionActionTypesCommand, bool>
{
    private readonly PortfolioContext m_context;

    public MigrateTransactionActionTypesCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(MigrateTransactionActionTypesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lst = TransactionActionTypeTool.ScanFromConstants();

            foreach (var item in lst)
            {
                var found = await m_context
                    .TransactionActionTypes
                    .FirstOrDefaultAsync(x => x.Code == item.Code, cancellationToken);

                if (found == null)
                {
                    m_context.TransactionActionTypes.Add(item);
                }
                else
                {
                    found.Name = item.Name;
                    found.Description = item.Description;
                    found.Category = item.Category;
                }
            }
        
            await m_context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch 
        {
            return false;
        }
    }
}