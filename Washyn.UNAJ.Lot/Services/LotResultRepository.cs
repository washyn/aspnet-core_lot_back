using Acme.BookStore.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Washyn.UNAJ.Lot.Controllers;
using Washyn.UNAJ.Lot.Data;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace Washyn.UNAJ.Lot
{
    public interface ILotResultRepository : IRepository<Sorteo>
    {
        Task<long> GetCountAsync(string? filter = null);
        Task<List<DocenteRoleData>> GetPagedListAsync(string? filter = null, int skipCount = 0, int maxResultCount = int.MaxValue, string sorting = null);
    }

    public class LotResultRepository : EfCoreRepository<LotDbContext, Sorteo>, ILotResultRepository
    {
        public LotResultRepository(IDbContextProvider<LotDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<DocenteRoleData>> GetPagedListAsync(string? filter = null, int skipCount = 0, int maxResultCount = int.MaxValue, string sorting = null)
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

        protected virtual IQueryable<DocenteRoleData> AplyFilter(IQueryable<DocenteRoleData> query, string? filter = null)
        {
            return query.WhereIf(!string.IsNullOrEmpty(filter), a => a.FullName.ToLower().Contains(filter.ToLower()));
        }

        public async Task<IQueryable<DocenteRoleData>> GetQueryableAsync()
        {
            var dbContext = await GetDbContextAsync();
            var queryable = from sorteo in dbContext.Sorteo
                            join docente in dbContext.Docentes on sorteo.DocenteId equals docente.Id
                            join rol in dbContext.Rols on sorteo.RolId equals rol.Id
                            // join grado in dbContext.Grados on docente.Id equals grado.Id

                            select new DocenteRoleData
                            {
                                Id = docente.Id,
                                Dni = docente.Dni,
                                ApellidoMaterno = docente.ApellidoMaterno,
                                ApellidoPaterno = docente.ApellidoPaterno,
                                Nombre = docente.Nombre,
                                FullName = docente.Nombre + " " + docente.ApellidoPaterno + " " + docente.ApellidoMaterno,
                                Genero = docente.Genero,
                                GradoId = docente.GradoId,
                                Area = docente.Area,
                                CreationTime = docente.CreationTime,
                                CreatorId = docente.CreatorId,
                                DeleterId = docente.DeleterId,
                                DeletionTime = docente.DeletionTime,
                                IsDeleted = docente.IsDeleted,
                                LastModificationTime = docente.LastModificationTime,
                                LastModifierId = docente.LastModifierId,
                                // GradoName = grado.Nombre,
                                // GradoPrefix = grado.Prefix,
                                RolName = rol.Nombre
                            };
            return queryable;
        }
    }
}