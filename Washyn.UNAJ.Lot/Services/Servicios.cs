using System.Linq.Dynamic.Core;
using Acme.BookStore.Entities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Washyn.UNAJ.Lot;
using Washyn.UNAJ.Lot.Services;

namespace Acme.BookStore.Services;

/// <summary>
/// Crud de docentes
/// </summary>
public class DocenteAppService : CrudAppService<Docente, DocenteDto, DocenteWithLookup, Guid, DocenteFilter,
    CreateUpdateDocenteDto, CreateUpdateDocenteDto>
{
    private readonly IDocenteRepository docenteRepository;

    public DocenteAppService(
        IRepository<Docente, Guid> repository,
        IDocenteRepository docenteRepository) : base(repository)
    {
        this.docenteRepository = docenteRepository;
    }

    public override async Task<DocenteDto> CreateAsync(CreateUpdateDocenteDto input)
    {
        var exists = await Repository.AnyAsync(a => a.Dni == input.Dni);
        if (exists)
        {
            throw new UserFriendlyException($"El numero de documento {input.Dni} ya se encuentra registrado.");
        }

        return await base.CreateAsync(input);
    }

    public override async Task<PagedResultDto<DocenteWithLookup>> GetListAsync(DocenteFilter input)
    {
        var totalCount = await docenteRepository.GetCountAsync(input.Filter);
        var data = await docenteRepository.GetPagedListAsync(input.Filter, input.SkipCount, input.MaxResultCount, input.Sorting);
        return new PagedResultDto<DocenteWithLookup>(totalCount, data);
    }
}

/// <summary>
/// Servicio se selects...
/// </summary>
public class SelectAppService : ApplicationService
{
    private readonly IRepository<Rol> _rolRepository;
    private readonly IRepository<Docente, Guid> _docenteRepository;
    private readonly IRepository<Curso, Guid> _cursoRepository;
    private readonly IRepository<Grado, Guid> _gradoRepository;

    public SelectAppService(IRepository<Rol> rolRepository,
        IRepository<Docente, Guid> docenteRepository,
        IRepository<Curso, Guid> cursoRepository,
        IRepository<Grado, Guid> gradoRepository)
    {
        _rolRepository = rolRepository;
        _docenteRepository = docenteRepository;
        _cursoRepository = cursoRepository;
        _gradoRepository = gradoRepository;
    }

    public async Task<List<LookupDto>> GetRol()
    {
        var data = await _rolRepository.GetListAsync();
        return data.Select(a => new LookupDto()
        {
            Id = a.Id,
            DisplayName = a.Nombre
        }).ToList();
    }

    public async Task<List<LookupDto>> GetDocente()
    {
        var data = await _docenteRepository.GetListAsync();
        return data.Select(a => new LookupDto()
        {
            Id = a.Id,
            DisplayName = a.ApellidoPaterno + " " + a.ApellidoMaterno + " " + a.Nombre
        }).ToList();
    }

    public async Task<List<LookupDto>> GetGrado()
    {
        var data = await _gradoRepository.GetListAsync();
        return data.Select(a => new LookupDto()
        {
            Id = a.Id,
            DisplayName = a.Nombre,
            AlternativeText = a.Prefix
        }).ToList();
    }

    public async Task<List<LookupDto>> GetCurso()
    {
        var data = await _cursoRepository.GetListAsync();
        return data.Select(a => new LookupDto()
        {
            Id = a.Id,
            DisplayName = a.Nombre,
        }).ToList();
    }
}