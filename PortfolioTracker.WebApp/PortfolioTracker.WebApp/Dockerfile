FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PortfolioTracker.WebApp/PortfolioTracker.WebApp.csproj", "PortfolioTracker.WebApp/"]
RUN dotnet restore "PortfolioTracker.WebApp/PortfolioTracker.WebApp.csproj"
COPY . .
WORKDIR "/src/PortfolioTracker.WebApp"
RUN dotnet build "PortfolioTracker.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PortfolioTracker.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortfolioTracker.WebApp.dll"]
