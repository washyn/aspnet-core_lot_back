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
    private readonly IParticipanteRepository participanteRepository;
    private readonly IRepository<Rol> _rolRepository;

    public ComisionAppService(IRepository<Comision, Guid> repository,
        IComisionRepository comisionRepository,
        IParticipanteRepository participanteRepository,
        IRepository<Rol> rolRepository) : base(repository)
    {
        _comisionRepository = comisionRepository;
        this.participanteRepository = participanteRepository;
        _rolRepository = rolRepository;
    }

    public async Task<List<DocenteLookup>> GetDocente()
    {
        return await _comisionRepository.GetAll();
    }

    public async Task<ComisionWithRoles> GetWithDetails(Guid comisionId)
    {
        var temp = await _comisionRepository.GetAllWithRoles();
        var comision = temp.First(a => a.Id == comisionId);
        return ObjectMapper.Map<Comision, ComisionWithRoles>(comision);
    }

    /// <summary>
    /// Return all comisions with roles.
    /// </summary>
    /// <returns></returns>
    public async Task<List<ComisionWithRoles>> GetAllWithDetails()
    {
        var temp = await _comisionRepository.GetAllWithRoles();
        return ObjectMapper.Map<List<Comision>, List<ComisionWithRoles>>(temp);
    }

    public async Task AssignToComision(Guid comisionId, List<Guid> docentes)
    {
        await _comisionRepository.UpdateParticipantes(comisionId, docentes);
    }

    public async Task<List<DocenteLookup>> GetParticipantes(Guid comisionId)
    {
        return await participanteRepository.GetAllParticipantes(comisionId);
    }

    // public async Task AssignToComision(List<Guid> data)
    // {
    //     // validate per element...
    // }

    [Obsolete]
    public async Task DeleteIntegrante(Guid integranteId, Guid comisionId)
    {
    }

    public async Task AddRol(AddRol model)
    {
        await _rolRepository.InsertAsync(new Rol()
        {
            Id = GuidGenerator.Create(),
            ComisionId = model.ComisionId,
            Nombre = model.Nombre
        });
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


public class AddRol : IEntityDto
{
    [Required]
    public string Nombre { get; set; }

    [Required]
    public Guid ComisionId { get; set; }
}

public class ComisionDto : EntityDto<Guid>
{
    [Required]
    public string Nombre { get; set; }
}

public class ComisionWithRoles : EntityDto<Guid>
{
    public string Nombre { get; set; }
    public List<RolDto> Rols { get; set; }
}

public class DocenteLookup : EntityDto<Guid>
{
    public string FullName { get; set; }
}

public interface IComisionRepository : IRepository<Comision, Guid>
{
    Task UpdateParticipantes(Guid comisionId, List<Guid> docentes);
    Task<List<DocenteLookup>> GetAll();
    Task<List<Comision>> GetAllWithRoles();
}

public class ComisionRepository : EfCoreRepository<LotDbContext, Comision, Guid>, IComisionRepository
{
    public ComisionRepository(IDbContextProvider<LotDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task UpdateParticipantes(Guid comisionId, List<Guid> docentes)
    {
        var dbContext = await GetDbContextAsync();

        await dbContext
            .Participantes
            .Where(a => a.ComisionId == a.ComisionId)
            .ExecuteDeleteAsync();

        var data = docentes.Select(a => new Participante
        {
            ComisionId = comisionId,
            DocenteId = a
        });

        await dbContext.Participantes.AddRangeAsync(data);
    }

    /// <summary>
    /// Get comision with roles.
    /// </summary>
    /// <returns></returns>
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

    public async Task<List<Comision>> GetAllWithRoles()
    {
        var dbContext = await GetDbContextAsync();
        var queryable = dbContext.Comisions.Include(a => a.Rols);
        var tempRes = await queryable.ToListAsync();
        return tempRes;
    }
}
