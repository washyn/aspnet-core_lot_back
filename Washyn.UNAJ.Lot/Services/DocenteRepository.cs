using System.Linq.Dynamic.Core;
using Acme.BookStore.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Washyn.UNAJ.Lot.Data;

namespace Washyn.UNAJ.Lot.Services
{
    public interface IDocenteRepository : IRepository<Docente, Guid>
    {
        Task<long> GetCountAsync(string? filter = null);
        Task<List<DocenteWithLookup>> GetPagedListAsync(string? filter = null, int skipCount = 0, int maxResultCount = int.MaxValue, string sorting = null);
        Task<DocenteWithLookup> GetWithDetails(Guid id);
    }

    public class DocenteRepository : EfCoreRepository<LotDbContext, Docente, Guid>, IDocenteRepository
    {
        public DocenteRepository(IDbContextProvider<LotDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<DocenteWithLookup>> GetPagedListAsync(string? filter = null, int skipCount = 0, int maxResultCount = int.MaxValue, string sorting = null)
        {
            var query = AplyFilter(await GetQueryableAsync(), filter);
            query = string.IsNullOrEmpty(sorting) ? query : query.OrderBy(sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync();
        }

        public async Task<long> GetCountAsync(string? filter = null)
        {
            var query = AplyFilter(await GetQueryableAsync(), filter);
            return await query.LongCountAsync();
        }

        protected virtual IQueryable<DocenteWithLookup> AplyFilter(IQueryable<DocenteWithLookup> query, string? filter = null)
        {
            return query.WhereIf(!string.IsNullOrEmpty(filter), a => a.FullName.ToLower().Contains(filter.ToLower()));
        }

        public async Task<IQueryable<DocenteWithLookup>> GetQueryableAsync()
        {
            var dbContext = await GetDbContextAsync();
            var queryable = from docente in dbContext.Docentes
                            join grado in dbContext.Grados on docente.GradoId equals grado.Id
                            select new DocenteWithLookup
                            {
                                Id = docente.Id,
                                Dni = docente.Dni,
                                ApellidoMaterno = docente.ApellidoMaterno,
                                ApellidoPaterno = docente.ApellidoPaterno,
                                Nombre = docente.Nombre,
                                FullName = docente.Nombre + " " + docente.ApellidoPaterno + " " + docente.ApellidoMaterno,
                                Genero = docente.Genero,
                                GradoId = docente.GradoId,
                                GradoName = grado.Nombre,
                                GradoPrefix = grado.Prefix,
                                Area = docente.Area,
                                CreationTime = docente.CreationTime,
                                CreatorId = docente.CreatorId,
                                DeleterId = docente.DeleterId,
                                DeletionTime = docente.DeletionTime,
                                IsDeleted = docente.IsDeleted,
                                LastModificationTime = docente.LastModificationTime,
                                LastModifierId = docente.LastModifierId,
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