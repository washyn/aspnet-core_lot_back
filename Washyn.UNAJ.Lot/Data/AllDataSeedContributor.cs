using Acme.BookStore.Entities;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Washyn.UNAJ.Lot.Models;
using Washyn.UNAJ.Lot.Services;

namespace Washyn.UNAJ.Lot.Data;

public class AppSettingContributor : IDataSeedContributor, ITransientDependency
{
    public AppSettingContributor(IRepository<AppSettings> repository,
        IJsonSerializer jsonSerializer,
        IOptionsSnapshot<DocumentOptions> optionsSnapshot)
    {
        Repository = repository;
        JsonSerializer = jsonSerializer;
        OptionsSnapshot = optionsSnapshot.Value;
    }

    public IRepository<AppSettings> Repository { get; }
    public IJsonSerializer JsonSerializer { get; }
    public DocumentOptions OptionsSnapshot { get; }

    public async Task SeedAsync(DataSeedContext context)
    {
        var stringConfig = JsonSerializer.Serialize(OptionsSnapshot);
        var data = new List<AppSettings>()
        {
            new AppSettings()
            {
                Key = ConfiguracionConts.GENERAL_OPTIONS,
                Value = stringConfig
            },
        };

        foreach (var item in data)
        {
            if (!await Exists(item.Key))
            {
                await Repository.InsertAsync(new AppSettings()
                {
                    Key = item.Key,
                    Value = item.Value
                });
            }
        }
    }

    private async Task<bool> Exists(string name)
    {
        return await Repository.AnyAsync(a => a.Key == name);
    }
}


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
    private readonly IRepository<Rol> _repository;

    public RoleDataSeedContributor(IRepository<Rol> repository)
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
            // if (!await Exists(item.Nombre))
            // {
            //     await _repository.InsertAsync(new Rol()
            //     {
            //         Nombre = item.Nombre,
            //     });
            // }
        }
    }

    private async Task<bool> Exists(string name)
    {
        return await _repository.AnyAsync(a => a.Nombre == name);
    }
}


public class ComisionDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Comision, Guid> _repository;

    public ComisionDataSeedContributor(IRepository<Comision, Guid> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var data = new List<Comision>()
        {
            new Comision()
            {
                Nombre = "COMISIÓN DE ELABORACIÓN",
            },
            new Comision()
            {
                Nombre = "COMISIÓN JURADO DE AULA"
            },
            // se puede agregar mas...
        };

        foreach (var item in data)
        {
            if (!await Exists(item.Nombre))
            {
                await _repository.InsertAsync(new Comision()
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