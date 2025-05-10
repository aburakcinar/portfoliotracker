//using FinanceData.Business.Api;
//using FinanceData.Business.DataStore;
//using Microsoft.EntityFrameworkCore;

//namespace FinanceData.Business.Services.ExchangeRateProviders;

//internal class DatabaseExchangeRateProvider : BaseRateProvider
//{
//    private readonly IFinansDataContext m_context;

//    public DatabaseExchangeRateProvider(IFinansDataContext context)
//    {
//        m_context = context;
//    }

//    public override int Order => 10;

//    public override async Task<decimal> GetRateAsync(CurrencyRateQueryModel query)
//    {
//        var baseCurrency = query.Base;
//        var targetCurrency = query.Target;
//        var date = query.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
        
//        var item = await m_context
//            .CurrencyExchangeRatios
//            .FirstOrDefaultAsync(x => 
//                x.BaseCurrency == baseCurrency && 
//                x.TargetCurrency == targetCurrency && 
//                x.Date >= date &&
//                x.Date < date.AddDays(1));

//        if (item == null)
//        {
//            return await ExecuteParentAsync(query);
//        }
        
//        return item.Rate;
//    }
//} 
