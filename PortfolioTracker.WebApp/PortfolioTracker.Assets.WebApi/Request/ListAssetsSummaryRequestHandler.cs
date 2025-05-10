using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Data.Models;

namespace PortfolioTracker.Assets.WebApp.Requests;

public static class ListAssetsSummaryRequestExtensions
{
    public static void MapListAssetsSummary(this IEndpointRouteBuilder app)
    {
        app.MapGet(@"/summary", ListAssetsSummary);
    }
    public static async Task<IResult> ListAssetsSummary(
        IMediator mediator)
    {
        var result = await mediator.Send(new ListAssetsSummaryRequest());
        return TypedResults.Ok(result);
    }
}

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
    private readonly IPortfolioContext m_context;

    public ListAssetsSummaryRequestHandler(IPortfolioContext context)
    {
        m_context = context;
    }

    public async Task<IEnumerable<AssetSummaryModel>> Handle(ListAssetsSummaryRequest request, CancellationToken cancellationToken)
    {
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

        await Task.CompletedTask;

        return result;
    }
}