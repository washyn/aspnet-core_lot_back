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

/// <summary>
/// Servicio para cru de comision y detalles
/// </summary>
public class ComisionAppService : CrudAppService<Comision, ComisionDto, Guid, PagedAndSortedResultRequestDto>
{
    private readonly IComisionRepository _comisionRepository;
    private readonly IRepository<Rol> _rolRepository;

    public ComisionAppService(IRepository<Comision, Guid> repository,
        IComisionRepository comisionRepository,
        IRepository<Rol> rolRepository) : base(repository)
    {
        _comisionRepository = comisionRepository;
        _rolRepository = rolRepository;
    }

    /// <summary>
    /// Retorna una comision con sus roles.
    /// </summary>
    /// <param name="comisionId"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Agrega un rol a una comision.
    /// </summary>
    /// <param name="model"></param>
    public async Task AddRol(AddRol model)
    {
        await _rolRepository.InsertAsync(new Rol()
        {
            Id = GuidGenerator.Create(),
            ComisionId = model.ComisionId,
            Nombre = model.Nombre
        });
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
