using System.ComponentModel.DataAnnotations;
using Acme.BookStore.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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
    
    public async Task RemoveRol(Guid rolId)
    {
        var rol = await _rolRepository.GetAsync(a => a.Id == rolId);
        await _rolRepository.DeleteAsync(rol);
    }
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