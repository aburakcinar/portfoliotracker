//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using PortfolioTracker.WebApp.Business.Commands.BankAccountEntity;

//namespace PortfolioTracker.WebApp.Controllers;

//[ApiController]
//[Route(@"api/[controller]")]
//public class BankAccountController : ControllerBase
//{
//    private readonly IMediator m_mediator;

//    public BankAccountController(IMediator mediator)
//    {
//        m_mediator = mediator;
//    }

//    [HttpGet(@"health")]
//    public IActionResult Health()
//    {
//        return Ok(this.GetType().Name);
//    }

//    [HttpGet(@"list")]
//    public async Task<IEnumerable<BankAccountModel>> ListAsync()
//    {
//        return await m_mediator.Send(new ListBankAccountsRequest());
//    }
    
//    [HttpGet(@"get/{id}")]
//    public async Task<BankAccountModel?> GetAsync([FromRoute] Guid id)
//    {
//        return await m_mediator.Send(new GetBankAccountRequest{ Id = id});
//    }
    
//    [HttpPost]
//    public async Task<bool> CreateAsync(CreateBankAccountCommand command)
//    {
//        return await m_mediator.Send(command);
//    }
    
    
//}