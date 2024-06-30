using Acme.BookStore.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Washyn.UNAJ.Lot.Data;

namespace Washyn.UNAJ.Lot.Services;

public interface IComisionRepository : IRepository<Comision, Guid>
{
    Task<List<Comision>> GetAllWithRoles();
}

public class ComisionRepository : EfCoreRepository<LotDbContext, Comision, Guid>, IComisionRepository
{
    public ComisionRepository(IDbContextProvider<LotDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
    
    public async Task<List<Comision>> GetAllWithRoles()
    {
        var dbContext = await GetDbContextAsync();
        var queryable = dbContext.Comisions.Include(a => a.Rols);
        var tempRes = await queryable.ToListAsync();
        return tempRes;
    }
}