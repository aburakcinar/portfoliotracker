using MediatR;

namespace PortfolioTracker.WebApp.Services;

public interface IDbSeedService
{
    Task SeedAsync();
}

public sealed class DbSeedService : IDbSeedService
{
    private readonly IMediator m_mediator;
    
    private readonly IAssetService m_assetService;
    

    public DbSeedService(
        IMediator mediator,
        IAssetService assetService)
    {
        m_mediator = mediator;
        m_assetService = assetService;
    }

    public async Task SeedAsync()
    {
        // await SeedAssetsAsync();
        // await m_mediator.Send(new ImportBankAccountCommand());
        // await m_mediator.Send(new ImportTransactionsCommand());

        await Task.CompletedTask;
    }


}