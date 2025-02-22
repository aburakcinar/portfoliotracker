using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.WebApp.Business.Commands.BankTransactionEntity;
using PortfolioTracker.WebApp.Business.Requests.BankTransactionEntity;
using PortfolioTracker.WebApp.Business.Requests.TransactionActionTypeEntity;
using PortfolioTracker.WebApp.DataStore;
using PortfolioTracker.WebApp.Tools;

namespace PortfolioTracker.WebApp.Controllers;

[Route(@"api/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private readonly IMediator m_mediator;

    public TransactionController(IMediator mediator)
    {
        m_mediator = mediator;
    }

    [HttpPost]
    public async Task<bool> Add([FromBody] AddTransactionCommand command)
    {
        return await m_mediator.Send(command);
    }

    [HttpGet(@"listbybankaccount/{bankAccountId}")]
    public async Task<IEnumerable<BankTransactionGroupModel>> ListTransactions([FromRoute]Guid bankAccountId)
    {
        return await m_mediator.Send(new ListBankAccountTransactionsRequest {BankAccountId = bankAccountId});
    }

    [HttpGet(@"scanactiontypes")]
    public IEnumerable<TransactionActionType> ScanTransactionActionTypes()
    {
        return TransactionActionTypeTool.ScanFromConstants();
    }

    [HttpGet(@"actiontypes")]
    public async Task<IEnumerable<TransactionActionType>> ListActionTypesAsync()
    {
        return await m_mediator.Send(new ListTransactionActionTypesRequest());
    }
}