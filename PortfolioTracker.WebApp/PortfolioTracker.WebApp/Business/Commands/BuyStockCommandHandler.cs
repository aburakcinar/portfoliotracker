using MediatR;

namespace PortfolioTracker.WebApp.Business.Commands;

public sealed class BuyStockCommand : IRequest<bool>
{
   

}


public class BuyStockCommandHandler : IRequestHandler<BuyStockCommand, bool>
{
    public Task<bool> Handle(BuyStockCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}