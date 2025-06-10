
var builder = DistributedApplication.CreateBuilder(args);

#region <FinanceData PostgreSQL service>

var financeUsername = builder.AddParameter(@"finance-db-username", secret: true);
var financePassword = builder.AddParameter(@"finance-db-password", secret: true);
var financeDatabasePath = builder.AddParameter(@"finance-db-data-path", secret: true);
var financeDatabaseName = builder.AddParameter(@"finance-db-database-name", secret: true);

var financePsqlService = builder
    .AddPostgres(@"financedbserver", financeUsername, financePassword)
    .WithImage(@"timescale/timescaledb:latest-pg14")
    .PublishAsConnectionString()
    .WithDataBindMount(financeDatabasePath.Resource.Value, isReadOnly: false);

var financePsqlDatabase = financePsqlService
    .AddDatabase(@"financedb", databaseName: financeDatabaseName.Resource.Value);

var financedbmigrationService = builder
    .AddProject<Projects.Finance_Data_Migrations>(@"financedb-migrations")
    .WithReference(financePsqlDatabase)
    .WaitFor(financePsqlDatabase);

#endregion 

#region <PostgreSQL service>

var username = builder.AddParameter(@"portfolio-tracker-db-username", secret: true);
var password = builder.AddParameter(@"portfolio-tracker-db-password", secret: true);
var databasePath = builder.AddParameter(@"portfolio-tracker-db-data-path", secret: true);
var databaseName = builder.AddParameter(@"portfolio-tracker-db-database-name", secret: true);

var portfolioPsqlService = builder
    .AddPostgres(@"portfoliodbserver", username, password)
    .PublishAsConnectionString()
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050))
    .WithDataBindMount(databasePath.Resource.Value, isReadOnly: false);

var portfolioPsqlDatabase = portfolioPsqlService
    .AddDatabase(@"portfoliodb", databaseName: databaseName.Resource.Value);

var portfoliodbmigrationService = builder
    .AddProject<Projects.PortfolioTracker_Data_Migrations>(@"portfoliodb-migrations")
    .WithReference(portfolioPsqlDatabase)
    .WaitFor(portfolioPsqlDatabase);

#endregion

#region Services

var assetService = builder
    .AddProject<Projects.PortfolioTracker_Assets_WebApi>(@"assetservice")
    .WithReference(portfolioPsqlDatabase)
    .WaitForCompletion(portfoliodbmigrationService);

var bankAccountService = builder
    .AddProject<Projects.PortfolioTracker_BankAccount_WebApi>(@"bankaccountservice")
    .WithReference(portfolioPsqlDatabase)
    .WaitForCompletion(portfoliodbmigrationService);

var exchangeService = builder
    .AddProject<Projects.PortfolioTracker_Exchanges_WebApi>(@"exchangeservice")
    .WithReference(portfolioPsqlDatabase)
    .WaitForCompletion(portfoliodbmigrationService);

var transactionService = builder
    .AddProject<Projects.PortfolioTracker_Transaction_WebApi>(@"transactionservice")
    .WithReference(portfolioPsqlDatabase)
    .WaitForCompletion(portfoliodbmigrationService);

var importService = builder
    .AddProject<Projects.PortfolioTracker_Imports_WebApi>(@"importservice")
    .WithReference(portfolioPsqlDatabase)
    .WaitForCompletion(portfoliodbmigrationService);

var portfolioService = builder
    .AddProject<Projects.PortfolioTracker_Portfolio_WebApi>(@"portfolioservice")
    .WithReference(portfolioPsqlDatabase)
    .WaitForCompletion(portfoliodbmigrationService);

#endregion

#region Gateway

var gateway = builder
    .AddProject<Projects.PortfolioTracker_Gateway_Api>(@"gateway")
    .WithReference(assetService)
    .WithReference(bankAccountService)
    .WithReference(exchangeService)
    .WithReference(transactionService)
    .WithReference(importService)
    .WithReference(portfolioService)
    .WaitFor(assetService)
    .WaitFor(bankAccountService)
    .WaitFor(exchangeService)
    .WaitFor(transactionService)
    .WaitFor(importService)
    .WaitFor(portfolioService)
    .WithExternalHttpEndpoints();

#endregion

builder.AddProject<Projects.PortfolioTracker_Portfolio_WebApi>("portfoliotracker-portfolio-webapi");

builder.Build().Run();
