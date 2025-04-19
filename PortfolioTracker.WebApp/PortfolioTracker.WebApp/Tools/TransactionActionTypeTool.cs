using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.WebApp.Tools;

public static class TransactionActionTypeTool
{
    public static List<TransactionActionType> ScanFromConstants()
    {
        var type = typeof(TransactionActionTypes);

        var fields = type
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x is {  IsInitOnly: true })
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