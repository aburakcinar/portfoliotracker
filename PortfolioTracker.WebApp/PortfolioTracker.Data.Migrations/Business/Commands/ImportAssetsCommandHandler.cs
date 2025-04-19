using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortfolioTracker.Data.Migrations.Services;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Data.Migrations.Business.Commands;

public sealed class ImportAssetsCommand : IRequest<bool>
{
}

public sealed class ImportAssetsCommandHandler : IRequestHandler<ImportAssetsCommand, bool>
{
    private readonly ILogger<ImportAssetsCommandHandler> m_logger;
    private readonly IPortfolioContext m_context;

    public ImportAssetsCommandHandler(
        ILogger<ImportAssetsCommandHandler> logger,
        IPortfolioContext context
        )
    {
        m_logger = logger;
        m_context = context;
    }

    public async Task<bool> Handle(ImportAssetsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            m_logger.LogInformation("Start importing Assets...");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $@"{typeof(ApiDbInitializer).Namespace}.Resources.assets.database.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                return false;
            }

            // Deserialize JSON directly from a file
            using var sr = new StreamReader(stream);
            JsonSerializer serializer = new JsonSerializer();
            var assets = serializer.Deserialize(sr, typeof(List<Asset>)) as List<Asset>;

            if (assets == null)
            {
                return false;
            }

            var itemsExists = await m_context
                .Assets
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var itemsToImport = assets
                .Where(x => !itemsExists.Contains(x.Id))
                .Select(x => new Models.Asset
                {
                    Id = x.Id,
                    TickerSymbol = x.TickerSymbol,
                    ExchangeCode = x.ExchangeMic,
                    CurrencyCode = x.CurrencyCode,
                    AssetType = AssetTypes.Stock,
                    Name = x.Name,
                    Description = x.ExternalData?.FullName ?? string.Empty,
                    Isin = x.Isin,
                    Wkn = string.Empty,
                    WebSite = x.ExternalData?.Tag ?? string.Empty,
                    Created = DateTime.UtcNow,
                    Price = 0
                })
                .ToArray();

            m_context.Assets.AddRange(itemsToImport);

            await m_context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            m_logger.LogError(ex, "Error importing Assets.");
            return false;
        }
    }
}


public class Asset
{
    public Guid Id { get; set; }

    public string ExchangeSymbol { get; set; } = null!;
    public string ExchangeMic { get; set; } = null!;
    public string TickerSymbol { get; set; } = null!;
    public string Name { get; set; } = null!;

    public string CountryCode { get; set; } = null!;
    public string CurrencyCode { get; set; } = string.Empty;

    public string? PrimaryIndustry { get; set; }
    public string? SecondaryIndustry { get; set; }
    public string? TertiaryIndustry { get; set; }

    public string Isin { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public AssetExternalData? ExternalData { get; set; } = null;

    public SimplyWallStExternalData? SimplyWallStExternalData { get; set; } = null;
}

public class SimplyWallStExternalData
{
    public string Id { get; set; } = string.Empty;
}

public class AssetExternalData
{
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public int Id { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
}
