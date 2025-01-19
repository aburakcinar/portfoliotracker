using System.ComponentModel;
using System.Reflection;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Tools;

public static class TransactionActionTypeTool
{
    public static List<TransactionActionType> ScanFromConstants()
    {
        var type = typeof(TransactionActionTypes);

        var fields = type
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x is { IsLiteral: true, IsInitOnly: false })
            .ToList();
        
        var result = new List<TransactionActionType>();

        foreach (var field in fields)
        {
            var item = new TransactionActionType
            {
                Code = field.Name,
                Description = string.Empty,
                Category = TransactionActionTypeCategory.Unknown
            };
            
            var attributes = field.GetCustomAttributes(false);

            foreach (var attribute in attributes)
            {
                if (attribute is DescriptionAttribute descriptionAttribute)
                {
                    item.Description = descriptionAttribute.Description;
                }
                else if (attribute is CategoryAttribute categoryAttribute)
                {
                    item.Category = Enum.Parse<TransactionActionTypeCategory>(categoryAttribute.Category);
                }
            }
            
            result.Add(item);
        }
        
        return result;
    }
}