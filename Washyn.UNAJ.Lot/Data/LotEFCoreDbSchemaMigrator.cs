using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Washyn.UNAJ.Lot.Data;

public class LotEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public LotEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the LotDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<LotDbContext>()
            .Database
            .MigrateAsync();
    }
}
