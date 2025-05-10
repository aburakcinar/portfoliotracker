//using FinanceData.Business.Api;
//using FinanceData.Business.DataStore;
//using Microsoft.EntityFrameworkCore;

//namespace FinanceData.Business.Services.ExchangeRateProviders;

//internal class ReverseDatabaseExchangeRateProvider : BaseRateProvider
//{
//    private readonly IFinansDataContext m_context;

//    public ReverseDatabaseExchangeRateProvider(IFinansDataContext context)
//    {
//        m_context = context;
//    }

//    public override int Order => 11;

//    public override async Task<decimal> GetRateAsync(CurrencyRateQueryModel query)
//    {
//        var baseCurrency = query.Base;
//        var targetCurrency = query.Target;
//        var date = query.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
        
//        var item = await m_context
//            .CurrencyExchangeRatios
//            .FirstOrDefaultAsync(x => 
//                x.BaseCurrency == targetCurrency && 
//                x.TargetCurrency == baseCurrency && 
//                x.Date >= date &&
//                x.Date < date.AddDays(1));

//        if (item == null)
//        {
//            return await ExecuteParentAsync(query);
//        }
        
//        return Math.Round(1 / item.Rate, 4);
//    }
//} 
