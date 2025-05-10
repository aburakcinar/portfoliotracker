using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.BankAccount.WebApi.Requests;

public static class DeleteBankAccountEndpointExtension
{
    public static void MapDeleteBankAccount(this IEndpointRouteBuilder app)
    {
        app.MapDelete(@"/{id:guid}", DeleteBankAccount)
            .WithName(nameof(DeleteBankAccount));
    }

    private static async Task<IResult> DeleteBankAccount(
        Guid id,
        IMediator mediator
        )
    {
        var result = await mediator.Send(new DeleteBankAccountCommand { Id = id});
        return result ? TypedResults.Ok() : TypedResults.BadRequest();
    }
}


public sealed class DeleteBankAccountCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, bool>
{
    private readonly IPortfolioContext m_context;
    public DeleteBankAccountCommandHandler(IPortfolioContext context)
    {
        m_context = context;
    }
    public async Task<bool> Handle(DeleteBankAccountCommand command, CancellationToken cancellationToken)
    {
        var item = await m_context
            .BankAccounts
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (item is null)
        {
            return false;
        }

        m_context.BankAccounts.Remove(item);
        
        var result = await m_context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}
