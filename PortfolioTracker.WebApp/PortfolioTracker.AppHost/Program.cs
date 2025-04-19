
var builder = DistributedApplication.CreateBuilder(args);

//var configuration = builder.Configuration;

//var username = configuration["portfolio_tracker_db:username"];
//var password = configuration["portfolio_tracker_db:password"];
//var databaseName = configuration["portfolio_tracker_db:database_name"];
//var databasePath = configuration["portfolio_tracker_db:data_path"];

var username = builder.AddParameter("portfolio-tracker-db-username", secret: true);
var password = builder.AddParameter("portfolio-tracker-db-password", secret: true);
var databasePath = builder.AddParameter("portfolio-tracker-db-data-path", secret: true);
var databaseName = builder.AddParameter("portfolio-tracker-db-database-name", secret: true);

// PgAdmin port : 5050

var portfolioPsqlService = builder
    .AddPostgres(@"portfoliodbserver", username, password)
    .PublishAsConnectionString()
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050))
    .WithDataBindMount(databasePath.Resource.Value, isReadOnly: false);

var portfolioPsqlDatabase = portfolioPsqlService
    .AddDatabase("portfoliodb", databaseName: databaseName.Resource.Value);

var migrationService = builder
    .AddProject<Projects.PortfolioTracker_Data_Migrations>("migrations")
    .WithReference(portfolioPsqlDatabase)
    .WaitFor(portfolioPsqlDatabase);

var webApp = builder
    .AddProject<Projects.PortfolioTracker_WebApp>("portfoliotrackerwebapp")
    .WithReference(portfolioPsqlDatabase)
    .WithExternalHttpEndpoints()
    .WaitForCompletion(migrationService);
//
// builder.AddNpmApp("react", "../../portfolio_tracker_react")
//     .WithReference(webApp)
//     .WithEnvironment("BROWSER", "none")
//     .WithHttpEndpoint(env: "PORT")
//     .WithExternalHttpEndpoints();

builder.Build().Run();
