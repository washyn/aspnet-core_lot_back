using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Acme.BookStore.Controllers;

[Route("files")]
[RemoteService(isEnabled: true)]
public class ExcelTemplate : AbpControllerBase
{
    private readonly ILogger<ExcelTemplate> _logger;

    public ExcelTemplate(ILogger<ExcelTemplate> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    [Route("template")]
    public async Task<IRemoteStreamContent> GetFileTemplate()
    {
        var data = new List<TemplateDocenteModel>()
        {
            new TemplateDocenteModel()
            {
                Area = "Docencia",
                ApellidoMaterno = "apellido",
                ApellidoPaterno = "apellido",
                Nombre = "docente"
            }
        };
        // {"Nombre": "docente", "ApellidoPaterno": "apellido", "ApellidoMaterno": "apellido", "Especialidad": "Docencia", "$type": "TemplateDocenteModel"}
        // can be adjust fields size...
        var ms = new MemoryStream();
        var mapper = new ExcelMapper();
        await mapper.SaveAsync(ms, data, "Docentes");
        ms.Seek(0, SeekOrigin.Begin);
        return new RemoteStreamContent(ms, "Plantilla sorteo docentes.xlsx", Volo.Abp.Http.MimeTypes.Application.OctetStream);
    }

    // DONE:
    [HttpPost]
    [Route("load")]
    public async Task LoadData(IRemoteStreamContent content)
    {
        var data = new ExcelMapper(content.GetStream()) { HeaderRow = true, }.Fetch<TemplateDocenteModel>();
        foreach (var item in data)
        {
            _logger.LogInformation("{@item}", item);
        }
    }
}

public class TemplateDocenteModel
{
    public string Nombre { get; set; }
    [Column("Apellido paterno")]
    public string ApellidoPaterno { get; set; }
    [Column("Apellido materno")]
    public string ApellidoMaterno { get; set; }
    public string Area { get; set; }
}