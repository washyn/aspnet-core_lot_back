using Acme.BookStore.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Washyn.UNAJ.Lot.Data;

namespace Washyn.UNAJ.Lot.Services
{
    public interface IParticipanteRepository : IRepository<Participante>
    {
        Task<List<DocenteLookup>> GetAllParticipantes(Guid comisionId);
    }

    public class ParticipanteRepository : EfCoreRepository<LotDbContext, Participante>, IParticipanteRepository
    {
        public ParticipanteRepository(IDbContextProvider<LotDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<DocenteLookup>> GetAllParticipantes(Guid comisionId)
        {
            var dbContext = await GetDbContextAsync();

            var queryable = from participante in dbContext.Participantes
                            join docente in dbContext.Docentes on participante.DocenteId equals docente.Id
                            where comisionId == comisionId
                            select new DocenteLookup
                            {
                                Id = docente.Id,
                                FullName = docente.Nombre + " " + docente.ApellidoPaterno + " " + docente.ApellidoMaterno,
                            };

            return await queryable.ToListAsync();
        }
    }
}