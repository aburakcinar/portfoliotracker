FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PortfolioTracker.Transaction.WebApi/PortfolioTracker.Transaction.WebApi.csproj", "PortfolioTracker.Transaction.WebApi/"]
RUN dotnet restore "PortfolioTracker.Transaction.WebApi/PortfolioTracker.Transaction.WebApi.csproj"
COPY . .
WORKDIR "/src/PortfolioTracker.Transaction.WebApi"
RUN dotnet build "PortfolioTracker.Transaction.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PortfolioTracker.Transaction.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortfolioTracker.Transaction.WebApi.dll"]
