using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Data.Migrations.Services;

internal interface ISeedService
{
    Task SeedAsync(CancellationToken cancellationToken);
}

internal sealed class SeedService : ISeedService
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}