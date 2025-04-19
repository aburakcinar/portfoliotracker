using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Models;
using PortfolioTracker.WebApp.Business.Requests.Locales;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/[controller]")]
[ApiController]
public class LocaleController : ControllerBase
{
    private readonly IMediator m_mediator;
    
    public LocaleController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpGet("bycountrycode/{countryCode}")]
    public async Task<LocaleQueryModel?> GetLocaleByCountryCode([FromRoute] string countryCode)
    {
        return await m_mediator.Send(new GetLocalByCountryCodeRequest { CountryCode = countryCode });
    }

    [HttpGet("search")]
    public async Task<IEnumerable<LocaleQueryModel>> Search([FromQuery] string searchText, [FromQuery] int limit = 50)
    {
        return await m_mediator.Send(new SearchLocaleRequest { SearchText = searchText, Limit = limit });
    }
}