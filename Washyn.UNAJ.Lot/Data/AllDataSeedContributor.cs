using Acme.BookStore.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Data;

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