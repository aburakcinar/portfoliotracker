using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.AssetEntity;

public sealed class AssetSummaryModel
{
    public int AssetTypeId { get; set; }

    public required string AssetType { get; set; }

    public int Count { get; set; }
}

public sealed class ListAssetsSummaryRequest : IRequest<IEnumerable<AssetSummaryModel>>
{
}

public sealed class ListAssetsSummaryRequestHandler : IRequestHandler<ListAssetsSummaryRequest, IEnumerable<AssetSummaryModel>>
{
    private readonly PortfolioContext m_context;

    public ListAssetsSummaryRequestHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<AssetSummaryModel>> Handle(ListAssetsSummaryRequest request, CancellationToken cancellationToken)
    {
        var result = new List<AssetSummaryModel>();
        
        foreach (var assetType in  Enum.GetValues<AssetTypes>())
        {
            result.Add( new AssetSummaryModel
            {
                AssetTypeId = (int)assetType,
                AssetType = assetType.ToString(),
                Count = await m_context.Assets.CountAsync(x => x.AssetType == assetType,cancellationToken)
            });
        }
        
        return result;
    }
}