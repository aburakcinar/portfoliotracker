using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Requests.AssetEntity;

public sealed class AssetSummaryModel
{
    public int AssetTypeId { get; init; }

    public required string AssetType { get; init; }
    
    public required string Title { get; init; }

    public int Count { get; init; }
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
        // var result = new List<AssetSummaryModel>();
        //
        // foreach (var assetType in  Enum.GetValues<AssetTypes>())
        // {
        //     Type typeEnum = assetType.GetType();
        //     typeEnum.GetCustomAttribute()
        //     
        //     result.Add( new AssetSummaryModel
        //     {
        //         AssetTypeId = (int)assetType,
        //         AssetType = assetType.ToString(),
        //         LinkName = $@"{assetType.ToString().ToLower().Replace(@"commodity", @"commoditie")}s",
        //         Count = await m_context.Assets.CountAsync(x => x.AssetType == assetType,cancellationToken)
        //     });
        // }

        await Task.CompletedTask;
        
        var result = Enum.GetValues(typeof(AssetTypes))
            .Cast<AssetTypes>()
            .Select(e => new AssetSummaryModel
            {
                AssetTypeId = (int)e,
                AssetType= e.ToString(),
                Title = e.GetType()
                    .GetField(e.ToString())?
                    .GetCustomAttribute<DisplayAttribute>()?
                    .Name ?? e.ToString(),
                Count =  m_context.Assets.Count(x => x.AssetType == e)
            });
        
        return result;
    }
}