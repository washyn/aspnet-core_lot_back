﻿using Acme.BookStore.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Washyn.UNAJ.Lot.Data;

public class AllDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Grado, Guid> _gradoRepository;

    public AllDataSeedContributor(IRepository<Grado, Guid> gradoRepository)
    {
        _gradoRepository = gradoRepository;
    }
    
    public async Task SeedAsync(DataSeedContext context)
    {
        var data = new List<Grado>()
        {
            new Grado()
            {
                Nombre = "Doctor",
                Prefix = "Dr."
            },
            new Grado()
            {
                Nombre = "Magister",
                Prefix = "Mg."
            },
            new Grado()
            {
                Nombre = "Ingeniero",
                Prefix = "Ing."
            },
            new Grado()
            {
                Nombre = "Licenciado",
                Prefix = "Lic."
            }
        };
        
        foreach (var item in data)
        {
            if (!await Exists(item.Nombre))
            {
                await _gradoRepository.InsertAsync(new Grado()
                {
                    Nombre = item.Nombre,
                    Prefix = item.Prefix
                });
            }
        }
        
    }

    private async Task<bool> Exists(string name)
    {
        return await _gradoRepository.AnyAsync(a => a.Nombre == name);
    }
}

public class RoleDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Rol, Guid> _repository;

    public RoleDataSeedContributor(IRepository<Rol, Guid> repository)
    {
        _repository = repository;
    }
    public async Task SeedAsync(DataSeedContext context)
    {
        var data = new List<Rol>()
        {
            new Rol()
            {
                Nombre = "REVISO DE INGENIERIAS",
            },
            new Rol()
            {
                Nombre = "REVISOR DE SOCIALES"
            },
            new Rol()
            {
                Nombre = "REVISOR DE ESTILO"
            },
            new Rol()
            {
                Nombre = "ELABORADOR"
            },
            new Rol()
            {
                Nombre = "DIGITADOR"
            },
            new Rol()
            {
                Nombre = "DIGITADOR"
            },
            // se puede agregar mas... 
        };
        
        foreach (var item in data)
        {
            if (!await Exists(item.Nombre))
            {
                await _repository.InsertAsync(new Rol()
                {
                    Nombre = item.Nombre,
                });
            }
        }
    }

    private async Task<bool> Exists(string name)
    {
        return await _repository.AnyAsync(a => a.Nombre == name);
    }
}