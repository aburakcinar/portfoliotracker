using MediatR;
using PortfolioTracker.Data.Migrations.Business.Commands;
using PortfolioTracker.WebApp.Business.Commands.Migrations;

namespace PortfolioTracker.Data.Migrations.Services;

public interface ISeedService
{
    Task SeedAsync(CancellationToken cancellationToken);
}

internal sealed class SeedService : ISeedService
{
    private readonly IMediator m_mediator;

    public SeedService(IMediator mediator)
    {
        m_mediator = mediator;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await m_mediator.Send(new ImportExchangesCommand(), cancellationToken);
        await m_mediator.Send(new ImportLocalesCommand(), cancellationToken);
        await m_mediator.Send(new ImportCurrenciesFromLocalesCommand(), cancellationToken);
        await m_mediator.Send(new ImportTransactionActionTypesCommand(), cancellationToken);
        await m_mediator.Send(new ImportAssetsCommand(), cancellationToken);
    }
}
