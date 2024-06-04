using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Washyn.UNAJ.Lot.Data;

public class LotDbContextFactory : IDesignTimeDbContextFactory<LotDbContext>
{
    public LotDbContext CreateDbContext(string[] args)
    {

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<LotDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));

        return new LotDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
