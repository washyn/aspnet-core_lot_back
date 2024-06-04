using Acme.BookStore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Services;

public class DocenteAppService : CrudAppService<Docente, DocenteDto, Guid, PagedAndSortedResultRequestDto,
    CreateUpdateDocenteDto, CreateUpdateDocenteDto>
{
    public DocenteAppService(IRepository<Docente, Guid> repository) : base(repository)
    {
    }
}


public class SelectAppService : ApplicationService
{
    private readonly IRepository<Rol, Guid> _rolRepository;
    private readonly IRepository<Docente, Guid> _docenteRepository;
    private readonly IRepository<Curso, Guid> _cursoRepository;
    private readonly IRepository<Grado, Guid> _gradoRepository;

    public SelectAppService(IRepository<Rol, Guid> rolRepository,
        IRepository<Docente, Guid> docenteRepository,
        IRepository<Curso, Guid> cursoRepository,
        IRepository<Grado, Guid> gradoRepository)
    {
        _rolRepository = rolRepository;
        _docenteRepository = docenteRepository;
        _cursoRepository = cursoRepository;
        _gradoRepository = gradoRepository;
    }

    public async Task<List<SelectListItem>> GetRol()
    {
        var data = await _rolRepository.GetListAsync();
        return data.Select(a => new SelectListItem()
        {
            Text = a.Id.ToString(),
            Value = a.Nombre
        }).ToList();
    }
    
    public async Task<List<SelectListItem>> GetDocente()
    {
        var data = await _docenteRepository.GetListAsync();
        return data.Select(a => new SelectListItem()
        {
            Text = a.Id.ToString(),
            Value = a.ApellidoPaterno +" "+ a.ApellidoMaterno +" "+ a.Nombre
        }).ToList();
    }
    
    public async Task<List<SelectListItem>> GetGrado()
    {
        var data = await _gradoRepository.GetListAsync();
        return data.Select(a => new SelectListItem()
        {
            Text = a.Id.ToString(),
            Value = a.Nombre
        }).ToList();
    }
    
    public async Task<List<SelectListItem>> GetCurso()
    {
        var data = await _cursoRepository.GetListAsync();
        return data.Select(a => new SelectListItem()
        {
            Text = a.Id.ToString(),
            Value = a.Nombre
        }).ToList();
    }
}