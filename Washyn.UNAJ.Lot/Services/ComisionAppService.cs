using System.ComponentModel.DataAnnotations;
using Acme.BookStore.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Washyn.UNAJ.Lot.Data;

namespace Washyn.UNAJ.Lot.Services;

public class ComisionAppService : CrudAppService<Comision, ComisionDto, Guid, PagedAndSortedResultRequestDto>
{
    private readonly IComisionRepository _comisionRepository;

    public ComisionAppService(IRepository<Comision, Guid> repository, IComisionRepository comisionRepository) : base(repository)
    {
        _comisionRepository = comisionRepository;
    }

    public async Task<List<DocenteLookup>> GetDocente()
    {
        return await _comisionRepository.GetAll();
    }

    public async Task AssignToComision(List<AsignComisionDto> data)
    {
        // validate per element...
    }

    public async Task DeleteIntegrante(Guid integranteId, Guid comisionId)
    {
    }

    public async Task<List<DocenteLookup>> GetDataByComisionAsync(Guid comisionId)
    {
        var data = await _comisionRepository.GetAll();
        return data;
    }
}

public class AsignComisionDto
{
    public Guid DocenteId { get; set; }
    public Guid ComisionId { get; set; }
}

public class ComisionDto : EntityDto<Guid>
{
    [Required]
    public string Nombre { get; set; }
}

public class DocenteLookup : EntityDto<Guid>
{
    public string FullName { get; set; }
}

public interface IComisionRepository : IRepository<Comision, Guid>
{
    Task<List<DocenteLookup>> GetAll();
}

public class ComisionRepository : EfCoreRepository<LotDbContext, Comision, Guid>, IComisionRepository
{
    public ComisionRepository(IDbContextProvider<LotDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<DocenteLookup>> GetAll()
    {
        var dbContext = await GetDbContextAsync();
        var queryable = from docente in dbContext.Docentes
                        select new DocenteLookup()
                        {
                            Id = docente.Id,
                            // Dni = docente.Dni,
                            // ApellidoMaterno = docente.ApellidoMaterno,
                            // ApellidoPaterno = docente.ApellidoPaterno,
                            // Nombre = docente.Nombre,
                            FullName = docente.Nombre + " " + docente.ApellidoPaterno + " " + docente.ApellidoMaterno,
                        };
        return await queryable.ToListAsync();
    }
}
