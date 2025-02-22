using MediatR;
using PortfolioTracker.WebApp.Business.Commands.AssetEntity;
using PortfolioTracker.WebApp.Business.Commands.BankAccountEntity;
using PortfolioTracker.WebApp.Business.Commands.BankTransactionEntity;
using PortfolioTracker.WebApp.Business.Commands.Exchanges;
using PortfolioTracker.WebApp.Business.Commands.Locales;
using PortfolioTracker.WebApp.Business.Commands.Migrations;

namespace PortfolioTracker.WebApp.Services;

public interface IDbSeedService
{
    Task SeedAsync();
}

public sealed class DbSeedService : IDbSeedService
{
    private readonly IMediator m_mediator;
    
    private readonly IExchangeService m_exchangeService;
    private readonly ILocaleService m_localeService;
    private readonly IAssetService m_assetService;
    

    public DbSeedService(
        IMediator mediator,
        IExchangeService exchangeService, 
        ILocaleService localeService, IAssetService assetService)
    {
        m_mediator = mediator;
        m_exchangeService = exchangeService;
        m_localeService = localeService;
        m_assetService = assetService;
    }

    public async Task SeedAsync()
    {
        // await SeedExchangesAsync();
        //
        // await SeedLocalesAsync();
        //
        // await m_mediator.Send(new MigrateCurrenciesFromLocalesCommand());

        await SeedAssetsAsync();

        await m_mediator.Send(new MigrateTransactionActionTypesCommand());

        await m_mediator.Send(new ImportBankAccountCommand());

        //await m_mediator.Send(new ImportTransactionsCommand());
    }

    private async Task SeedExchangesAsync()
    {
        var result  = m_exchangeService.List();

        foreach (var item in result)
        {
            await m_mediator.Send(new UpsertExchangeCommand
            {
                Exchange = item
            });
        }
    }

    private async Task SeedLocalesAsync()
    {
        var result  = await m_localeService.ListAsync();

        if (result is null)
        {
            return;
        }
        
        foreach (var item in result)
        {
            await m_mediator.Send(new UpsertLocaleCommand
            {
                Locale = item
            });
        }
    }

    private async Task SeedAssetsAsync()
    {
        var result = await m_assetService.ListAsync();

        if (result is null)
        {
            return;
        }

        foreach (var item in result)
        {
            await m_mediator.Send(new ImportAssetCommand
            {
                AssetItem = item
            });
        }
    }
}