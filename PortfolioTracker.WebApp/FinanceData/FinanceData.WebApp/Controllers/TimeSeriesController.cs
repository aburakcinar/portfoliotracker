using FinanceData.Business.Api;
using FinanceData.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceData.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeSeriesController : ControllerBase
{
    private readonly ICurrencyRateService m_service;

    public TimeSeriesController(ICurrencyRateService service)
    {
        m_service = service;
    }

    [HttpGet]
    public async Task<GetCurrencyRatesTimeseriesResult> Get([FromQuery] GetCurrencyRatesTimeseriesQuery query)
    {
        return await m_service.GetTimeSeriesAsync(query);
    }

    [HttpGet]
    [Route("health")]
    public IActionResult Health()
    {
        return Ok(@"Healthy");
    }
}