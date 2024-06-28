using Acme.BookStore.Entities;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Washyn.UNAJ.Lot.Models;

namespace Washyn.UNAJ.Lot.Services
{
    // Sercvicio de aplicacion para modificar parametros en la generacion de los informes pdf(carta).
    public class ConfiguracionAppService : ApplicationService
    {
        public IRepository<AppSettings> Repository { get; }
        public IJsonSerializer JsonSerializer { get; }
        public DocumentOptions Options { get; }

        // add key value table...
        // load database first if not works then load from app settings
        public ConfiguracionAppService(IRepository<AppSettings> repository,
            IJsonSerializer jsonSerializer,
            IOptionsSnapshot<DocumentOptions> options)
        {
            Repository = repository;
            JsonSerializer = jsonSerializer;
            Options = options.Value;
        }

        // add set
        // try add config module in database...
        public async Task CreateConfig(DocumentOptions documentOptions)
        {
            // set to db... as json ...
            // create or update...

            var setting = await Repository.GetAsync(a => a.Key == ConfiguracionConts.GENERAL_OPTIONS);
            if (setting is null)
            {
                await Repository.InsertAsync(new AppSettings
                {
                    Key = ConfiguracionConts.GENERAL_OPTIONS,
                    Value = JsonSerializer.Serialize(documentOptions),
                });
            }
            else
            {
                var text = JsonSerializer.Serialize(documentOptions);
                setting.Value = text;
                await Repository.UpdateAsync(setting);
            }
        }

        // get data from db...
        // return value
        public async Task<DocumentOptions> GetConfig()
        {
            var setting = await Repository.GetAsync(a => a.Key == ConfiguracionConts.GENERAL_OPTIONS);
            if (setting is null)
            {
                return this.Options;
            }

            var opt = JsonSerializer.Deserialize<DocumentOptions>(setting.Value);
            if (opt is not null)
            {
                return opt;
            }

            Logger.LogWarning($"No se logro encontrar una configuracion para la applicacion.");
            return new DocumentOptions() { };
        }
    }

    public class ConfiguracionConts
    {
        public const string GENERAL_OPTIONS = nameof(GENERAL_OPTIONS);
    }
}