using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Services;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/[controller]")]
[ApiController]
public class LocaleController : ControllerBase
{
    private readonly ILocaleImporter m_localeImporter;
    
    public LocaleController(ILocaleImporter localeImporter)
    {
        m_localeImporter = localeImporter;
    }

    [HttpPost]
    public async Task<IActionResult> Scan()
    {
        await m_localeImporter.ScanAsync();

        return Ok();
    }
}