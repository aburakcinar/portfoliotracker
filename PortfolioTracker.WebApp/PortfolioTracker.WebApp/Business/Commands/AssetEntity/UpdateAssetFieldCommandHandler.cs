using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.WebApp.DataStore;

namespace PortfolioTracker.WebApp.Business.Commands.AssetEntity;

public sealed class UpdateAssetFieldCommand : IRequest<bool>
{
    public Guid AssetId { get; init; }

    public string FieldName { get; init; } = string.Empty;

    public string Value { get; init; } = string.Empty;
}

public sealed class UpdateAssetFieldCommandHandler : IRequestHandler<UpdateAssetFieldCommand, bool>
{
    private readonly PortfolioContext m_context;

    public UpdateAssetFieldCommandHandler(PortfolioContext context)
    {
        m_context = context;
    }

    public async Task<bool> Handle(UpdateAssetFieldCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var asset = await m_context.Assets.FirstOrDefaultAsync(x => x.Id == request.AssetId, cancellationToken);

            if (asset is not null)
            {
                var updated = true;
                switch (request.FieldName)
                {
                    case nameof(asset.Description):
                        asset.Description = request.Value;
                        break;
                    case nameof(asset.Isin):
                        asset.Isin = request.Value;
                        break;
                    case nameof(asset.Wkn):
                        asset.Wkn = request.Value;
                        break;
                    case nameof(asset.WebSite):
                        asset.WebSite = request.Value;
                        break;
                    default:
                        updated = false;
                        break;
                }

                if (updated)
                {
                    return (await m_context.SaveChangesAsync(cancellationToken)) > 0;
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}