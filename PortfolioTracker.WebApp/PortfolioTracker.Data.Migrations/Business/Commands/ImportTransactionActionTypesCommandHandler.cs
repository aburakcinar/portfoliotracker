using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Business.Commands.Migrations;

public sealed class ImportTransactionActionTypesCommand : IRequest<bool>
{
}

public sealed class ImportTransactionActionTypesCommandHandler : IRequestHandler<ImportTransactionActionTypesCommand, bool>
{
    private readonly ILogger<ImportTransactionActionTypesCommandHandler> m_logger;
    private readonly IPortfolioContext m_context;

    public ImportTransactionActionTypesCommandHandler(
        ILogger<ImportTransactionActionTypesCommandHandler> logger,
        IPortfolioContext context
        )
    {
        m_logger = logger;
        m_context = context;
    }

    public async Task<bool> Handle(ImportTransactionActionTypesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            m_logger.LogInformation("Start importing Transaction action types...");

            var lst = ScanFromConstants();

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

            var result = await m_context.SaveChangesAsync(cancellationToken);

            m_logger.LogInformation($@"End importing Transaction action types with {result} record.");

            return result > 0;
        }
        catch (Exception ex)
        {
            m_logger.LogError(message: "Error on importing Transaction action types.", exception: ex);
            return false;
        }
    }

    public static List<TransactionActionType> ScanFromConstants()
    {
        var type = typeof(TransactionActionTypes);

        var fields = type
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x is { IsInitOnly: true })
            .ToList();

        var result = new List<TransactionActionType>();

        foreach (var field in fields)
        {
            var item = new TransactionActionType
            {
                Code = field.Name,
                Name = string.Empty,
                Description = string.Empty,
                Category = TransactionActionTypeCategory.Unknown
            };

            var attributes = field.GetCustomAttributes(false);

            foreach (var attribute in attributes)
            {
                if (attribute is DisplayAttribute displayAttribute)
                {
                    item.Name = displayAttribute.Name ?? field.Name;
                    item.Description = displayAttribute.Description ?? string.Empty;
                    item.Category = Enum.Parse<TransactionActionTypeCategory>(displayAttribute.GroupName ?? TransactionActionTypeCategory.Unknown.ToString(), ignoreCase: true);
                }
            }

            result.Add(item);
        }

        return result;
    }
}