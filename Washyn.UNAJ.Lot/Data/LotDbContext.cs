using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Washyn.UNAJ.Lot.Data;

public class LotDbContext : AbpDbContext<LotDbContext>
{
    public LotDbContext(DbContextOptions<LotDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        /* Configure your own entities here */
    }
}
