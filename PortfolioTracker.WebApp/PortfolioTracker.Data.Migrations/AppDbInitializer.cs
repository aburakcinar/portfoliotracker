// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using PortfolioTracker.Data.Migrations.Services;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Data.Migrations;

public class ApiDbInitializer(
    ILogger<ApiDbInitializer> logger,
    IServiceProvider serviceProvider,
    IHostEnvironment hostEnvironment,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly ActivitySource _activitySource = new(hostEnvironment.ApplicationName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity(hostEnvironment.ApplicationName, ActivityKind.Client);

        logger.LogInformation(@"Start creation and migration of database...");

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioContext>();

            await EnsureDatabaseAsync(logger, dbContext, cancellationToken);
            await RunMigrationAsync(logger, dbContext, cancellationToken);
            await RunSeedAsync(logger, cancellationToken);

            logger.LogInformation(@"End creation and migration of database.");
        }
        catch (Exception ex)
        {
            logger.LogError(message: "Error on creation and migration database", exception: ex);
            activity?.AddException(ex);
            throw;
        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }

    private static async Task EnsureDatabaseAsync(
        ILogger<ApiDbInitializer> logger,
        PortfolioContext dbContext,
        CancellationToken cancellationToken
        )
    {
        logger.LogInformation("Database creation started...");

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
        logger.LogInformation("Database creation ended.");
    }

    private static async Task RunMigrationAsync(
        ILogger<ApiDbInitializer> logger,
        PortfolioContext dbContext,
        CancellationToken cancellationToken
        )
    {
        logger.LogInformation("Database migration started...");
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            //await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            //await transaction.CommitAsync(cancellationToken);
        });
        logger.LogInformation("Database migration ended.");
    }

    private async Task RunSeedAsync(
        ILogger<ApiDbInitializer> logger,
        CancellationToken cancellationToken
        )
    {
        logger.LogInformation("Database seed started...");

        using var scope = serviceProvider.CreateScope();
        var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();

        await seedService.SeedAsync(cancellationToken);
        
        logger.LogInformation("Database seed ended.");
    }
}
