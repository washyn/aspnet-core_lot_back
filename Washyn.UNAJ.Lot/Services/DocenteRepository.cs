using System.Linq.Dynamic.Core;
using Acme.BookStore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Washyn.UNAJ.Lot;
using Washyn.UNAJ.Lot.Data;

namespace Washyn.UNAJ.Lot.Services
{
    public interface IDocenteRepository : IRepository<Docente, Guid>
    {
        Task<long> GetCountAsync();
        Task<List<DocenteWithLookup>> GetPagedListAsync(int skipCount = 0, int maxResultCount = int.MaxValue, string sorting = null);
        Task<DocenteWithLookup> GetWithDetails(Guid id);
    }

    public class DocenteRepository : EfCoreRepository<LotDbContext, Docente, Guid>, IDocenteRepository
    {
        public DocenteRepository(IDbContextProvider<LotDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<DocenteWithLookup>> GetPagedListAsync(int skipCount = 0, int maxResultCount = int.MaxValue, string sorting = null)
        {
            var query = AplyFilter(await GetQueryableAsync());
            query = string.IsNullOrEmpty(sorting) ? query : query.OrderBy(sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync();
        }

        public async Task<long> GetCountAsync()
        {
            var query = AplyFilter(await GetQueryableAsync());
            return await query.LongCountAsync();
        }

        protected virtual IQueryable<DocenteWithLookup> AplyFilter(IQueryable<DocenteWithLookup> query)
        {
            return query;
        }

        public async Task<IQueryable<DocenteWithLookup>> GetQueryableAsync()
        {
            var dbContext = await GetDbContextAsync();
            var queryable = from docente in dbContext.Docentes
                            join grado in dbContext.Grados on docente.GradoId equals grado.Id
                            select new DocenteWithLookup
                            {
                                Id = docente.Id,
                                ApellidoMaterno = docente.ApellidoMaterno,
                                ApellidoPaterno = docente.ApellidoPaterno,
                                Nombre = docente.Nombre,
                                Genero = docente.Genero,
                                GradoId = docente.GradoId,
                                GradoName = grado.Nombre,
                                GradoPrefix = grado.Prefix,
                            };
            return queryable;
        }

        public async Task<DocenteWithLookup> GetWithDetails(Guid id)
        {
            var queryable = await GetQueryableAsync();
            return queryable.AsNoTracking().First(a => a.Id == id);
        }
    }


}