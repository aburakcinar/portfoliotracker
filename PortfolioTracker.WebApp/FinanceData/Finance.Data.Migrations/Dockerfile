# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["FinanceData/Finance.Data.Migrations/Finance.Data.Migrations.csproj", "FinanceData/Finance.Data.Migrations/"]
COPY ["PortfolioTracker.ServiceDefaults/PortfolioTracker.ServiceDefaults.csproj", "PortfolioTracker.ServiceDefaults/"]
COPY ["FinanceData/Finance.Data.Models/Finance.Data.Models.csproj", "FinanceData/Finance.Data.Models/"]
RUN dotnet restore "./FinanceData/Finance.Data.Migrations/Finance.Data.Migrations.csproj"
COPY . .
WORKDIR "/src/FinanceData/Finance.Data.Migrations"
RUN dotnet build "./Finance.Data.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Finance.Data.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finance.Data.Migrations.dll"]