using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Data.Migrations.Services;
using FinanceData.Business.DataStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Finance.Data.Migrations;

internal sealed class AppDbInitializer : BackgroundService
{
    private readonly ILogger<AppDbInitializer> m_logger;
    private readonly IServiceProvider m_serviceProvider;
    private readonly IHostApplicationLifetime m_hostApplicationLifetime;
    private readonly ActivitySource m_activitySource;
    public AppDbInitializer(
        ILogger<AppDbInitializer> logger,
        IServiceProvider serviceProvider,
        IHostEnvironment hostEnvironment,
        IHostApplicationLifetime hostApplicationLifetime
        )
    {
        m_logger = logger;
        m_serviceProvider = serviceProvider;
        m_hostApplicationLifetime = hostApplicationLifetime;
        m_activitySource = new(hostEnvironment.ApplicationName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = m_activitySource.StartActivity(nameof(AppDbInitializer), ActivityKind.Client);

        m_logger.LogInformation(@"Start creation and migration of database...");

        try
        {
            using var scope = m_serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FinansDataContext>();
            await EnsureDatabaseAsync(dbContext, stoppingToken);
            await RunMigrationAsync(dbContext, stoppingToken);
            //await RunSeedAsync(stoppingToken);

            m_logger.LogInformation(@"End creation and migration of database.");
        }
        catch (Exception ex)
        {
            m_logger.LogError(message: "Error on creation and migration database", exception: ex);
            activity?.AddException(ex);
            throw;
        }
        finally
        {
            // Stop the application
             m_hostApplicationLifetime.StopApplication();
        }
    }

    private async Task EnsureDatabaseAsync(
        FinansDataContext dbContext,
        CancellationToken cancellationToken
        )
    {
        m_logger.LogInformation("Database creation started...");

        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
        m_logger.LogInformation("Database migration ended.");
    }

    private async Task RunMigrationAsync(
        FinansDataContext dbContext,
        CancellationToken cancellationToken
        )
    {
        m_logger.LogInformation("Database migration started...");
        
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
        
        m_logger.LogInformation("Database migration ended.");
    }

    private async Task RunSeedAsync(
       CancellationToken cancellationToken
       )
    {
        m_logger.LogInformation("Database seed started...");

        using var scope = m_serviceProvider.CreateScope();
        var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();

        await seedService.SeedAsync(cancellationToken);

        m_logger.LogInformation("Database seed ended.");
    }
}
