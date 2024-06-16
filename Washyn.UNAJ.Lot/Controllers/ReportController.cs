using System.Globalization;
using Acme.BookStore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Http;
using Washyn.UNAJ.Lot.Models;
using Washyn.UNAJ.Lot.Services;
namespace Washyn.UNAJ.Lot.Controllers
{
    [Route("report")]
    [RemoteService]
    public class ReportController : AbpController
    {
        private readonly DocumentOptions options;
        private readonly IDocenteRepository docenteRepository;
        private readonly ILotResultRepository lotResultRepository;

        public ReportController(IOptionsMonitor<DocumentOptions> options,
            IDocenteRepository docenteRepository,
            ILotResultRepository lotResultRepository)
        {
            this.options = options.CurrentValue;
            this.docenteRepository = docenteRepository;
            this.lotResultRepository = lotResultRepository;
        }

        // lanzar todos los tipos de errores... de abp
        [HttpGet]
        [Route("error-frieldy-sample")]
        public async Task SampleErrorFriendly()
        {
            throw new UserFriendlyException("Error fliendlyyyyyyyy from backend...");
        }

        // this for single download...
        // [HttpGet]
        // [Route("sample")]
        // public async Task<IRemoteStreamContent> GetSamplePdfReport(Guid id)
        // {
        //     var model = await docenteRepository.GetWithDetails(id);
        //     var ms = new MemoryStream(GenerateDocument(model));
        //     return new RemoteStreamContent(ms, "Reporte ejemplo.pdf", MimeTypes.Application.Pdf);
        // }

        [HttpGet]
        [Route("all-lot-result")]
        public async Task<IRemoteStreamContent> GetAllPdfReport()
        {
            var sorteo = await lotResultRepository.GetPagedListAsync(null, 0, 1000, null);
            var ms = new MemoryStream(GenerateAllDocuments(sorteo));
            return new RemoteStreamContent(ms, "Reporte masivo.pdf", MimeTypes.Application.Pdf);
        }

        private byte[] GenerateAllDocuments(List<DocenteRoleData> docentes)
        {
            var sequenceInitial = options.SequenceStart;

            QuestPDF.Settings.License = LicenseType.Community;

            var culture = new CultureInfo("es-pe");

            var document = Document.Create(container =>
            {
                foreach (var docente in docentes)
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.MarginHorizontal(2.5f, Unit.Centimetre);
                        page.MarginTop(0.5f, Unit.Centimetre);
                        page.MarginBottom(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily("Arial"));
                        page.DefaultTextStyle(x => x.FontSize(11));

                        page.Background()
                            .AlignMiddle()
                            .AlignCenter()
                            .Rotate(-45f)
                            .Text("CONFIDENCIAL")
                            .Bold()
                            .FontSize(60)
                            .FontColor(Colors.Red.Lighten4)
                            .FontFamily("Arial")
                            ;

                        page.Header()
                            .Column(x =>
                            {
                                x.Item().Row(row =>
                                {
                                    row.Spacing(10);
                                    row.RelativeItem(2)
                                        .Width(100)
                                        .Image("unaj logo.png");
                                    row.RelativeItem(4)
                                        .DefaultTextStyle(a => a.FontSize(9))
                                        .DefaultTextStyle(a => a.FontFamily("Arial"))
                                        .Column(a =>
                                        {
                                            a.Item().Text("UNIVERSIDAD  NACIONAL  DE JULIACA").AlignCenter();
                                            a.Item().Text("CREADA  POR  LEY  N\u00b0  29074").AlignCenter().Italic();
                                            a.Item().Text("DIRECCIÓN DE ADMISIÓN").AlignCenter();
                                        });
                                    row.RelativeItem(2)
                                        .Width(75)
                                        .Image("direccion de admision logo.png");
                                });

                                x.Item().PaddingVertical(10).LineHorizontal(0.5f).LineColor(Colors.Black);
                            });

                        page.Content()
                            .Column(x =>
                            {
                                x.Spacing(5);
                                x.Item()
                                    .Text(options.YearName)
                                    .FontSize(10)
                                    .AlignCenter()
                                    .Italic();

                                x.Item().PaddingVertical(10).Text($"{options.FechaGenerada}").AlignEnd();

                                x.Item().PaddingBottom(10).Text($"CARTA Nº {sequenceInitial++}{options.NumeroCarta}").AlignStart().Underline().Bold();

                                x.Item().Text($"{MapGender(docente.Genero)}:").AlignStart();
                                x.Item().Text($"{docente.GradoPrefix} {docente.FullName}").AlignStart().Bold();
                                x.Item().PaddingBottom(10).Text($"PRESENTE.-").AlignStart().Underline();

                                x.Item().Text($"ASUNTO:  {options.Asunto}").Bold();
                                var text = @"De mi especial consideración;";

                                x.Item().PaddingVertical(5).Text(text);
                                x.Item().Text(t =>
                                {
                                    t.Justify();

                                    t.Span("Por medio del presesente documento me dirijo a su distinguida persona para expresarle un cordial saludo, asimismo informarle que este ");
                                    t.Span(options.FechaExamen).Bold().Underline();
                                    t.Span(" ");
                                    t.Span("se desarrollará el examen de admisión en su modalidad ");
                                    t.Span(options.Modalidad);
                                    t.Span(".");
                                });
                                x.Item().Text(t =>
                                {
                                    t.Justify();
                                    t.Span("Por lo anterior, esta dirección le invita a participar en la ");
                                    t.Span("COMISIÓN DE ELABORACIÓN").Bold();
                                    t.Span(" en calidad de ");
                                    t.Span(docente.RolName.ToUpper());
                                    t.Span(
                                        " de examen en el proceso en mención; asismismo, poner de su conocimiento que debera realizar coordinaciones con el equipo de su comisión.");
                                });
                                x.Item().PaddingBottom(10).Text(t =>
                                {
                                    t.Justify();
                                    t.Span(options.Despedida);
                                });

                                x.Item().Text($"Atentamente,").AlignCenter();
                            });

                        page.Footer()
                            .Column(x =>
                            {
                                x.Item().Text("C.C.: Archivo");
                                x.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Black);
                                x
                                    .Item()
                                    .AlignCenter()
                                    .Text(
                                        "Av. Nueva Zelandia N\u00b0 631 Urbanización la capilla Teléfono 328722-Juliaca - Perú")
                                    .FontSize(10);
                            });
                    });


                }
            });

            return document.GeneratePdf();
        }

        // IMPROVE: add extension method for add page and receve model ...
        private byte[] GenerateDocument(DocenteWithLookup docente)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var culture = new CultureInfo("es-pe");

            var data3 = """
                        Aprovecho la oportunidad para reiterarle los sentimientos de mi mayor consideracion y estima personal.
                        """;
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(2.5f, Unit.Centimetre);
                    page.MarginTop(0.5f, Unit.Centimetre);
                    page.MarginBottom(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily("Arial"));
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Background()
                        .AlignMiddle()
                        .AlignCenter()
                        .Rotate(-45f)
                        .Text("CONFIDENCIAL")
                        .Bold()
                        .FontSize(60)
                        .FontColor(Colors.Red.Lighten4)
                        .FontFamily("Arial")
                        ;

                    page.Header()
                        .Column(x =>
                        {
                            x.Item().Row(row =>
                            {
                                row.Spacing(10);
                                row.RelativeItem(2).Image("unaj logo.png");
                                row.RelativeItem(4)
                                    .DefaultTextStyle(a => a.FontSize(9))
                                    .DefaultTextStyle(a => a.FontFamily("Arial"))
                                    .Column(a =>
                                    {
                                        a.Item().Text("UNIVERSIDAD  NACIONAL  DE JULIACA").AlignCenter();
                                        a.Item().Text("CREADA  POR  LEY  N\u00b0  29074").AlignCenter().Italic();
                                        a.Item().Text("DIRECCIÓN DE ADMISIÓN").AlignCenter();
                                    });
                                row.RelativeItem(2)
                                    .Image("direccion de admision logo.png").FitWidth();
                            });

                            x.Item().PaddingVertical(10).LineHorizontal(0.5f).LineColor(Colors.Black);
                        });

                    page.Content()
                        .Column(x =>
                        {
                            x.Spacing(5);
                            x.Item()
                                .Text(options.YearName)
                                .FontSize(10)
                                .AlignCenter()
                                .Italic();

                            var textDate = DateTime.Now.ToString("D", culture);

                            x.Item().PaddingVertical(10).Text($"Juliaca, {textDate}").AlignEnd();

                            x.Item().PaddingBottom(10).Text($"CARTA Nº: {options.NumeroCarta}").AlignStart().Underline().Bold();

                            x.Item().Text($"{MapGender(docente.Genero)}:").AlignStart();
                            x.Item().Text($"{docente.GradoPrefix} {docente.FullName}").AlignStart().Bold();
                            x.Item().PaddingBottom(10).Text($"PRESENTE.-").AlignStart().Underline();

                            x.Item().Text($"ASUNTO:  {options.Asunto}").Bold();
                            var text = @"De mi especial consideración;";

                            x.Item().PaddingVertical(5).Text(text);
                            x.Item().Text(t =>
                            {
                                t.Justify();

                                t.Span("Por medio del presesente documento me dirijo a su distinguida persona para expresarle un cordial saludo, asimismo informarle que este ");
                                t.Span("domingo 31 de marzo ").Bold().Underline();
                                t.Span("se desarrollará el examen de admisión en su modalidad ");
                                t.Span(options.Modalidad);
                                t.Span(".");
                            });
                            x.Item().Text(t =>
                            {
                                t.Justify();
                                t.Span("Por lo anterior, esta dirección le invita a participar en la ");
                                t.Span("COMISIÓN DE ELABORACIÓN").Bold();
                                t.Span(" en calidad de ");
                                t.Span("DIGITADOR");
                                t.Span(
                                    " de examen en el proceso en mención; asismismo, poner de su conocimiento que debera realizar coordinaciones con el responsable de su comisión.");
                            });
                            x.Item().PaddingBottom(10).Text(t =>
                            {
                                t.Justify();
                                t.Span(data3);
                            });

                            x.Item().Text($"Atentamente,").AlignCenter();
                        });

                    page.Footer()
                        .Column(x =>
                        {
                            x.Item().Text("C.C.: Archivo");
                            x.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Black);
                            x
                                .Item()
                                .AlignCenter()
                                .Text(
                                    "Av. Nueva Zelandia N\u00b0 631 Urbanización la capilla Teléfono 328722-Juliaca - Perú")
                                .FontSize(10);
                        });
                });


            });

            return document.GeneratePdf();
        }

        private string MapGender(Gender gender)
        {
            switch (gender)
            {
                case Gender.Female:
                    return "Sra.";
                case Gender.Male:
                    return "Sr.";
                case Gender.Unknow:
                    return "Sr.";
                default:
                    return "Sr.";
            }
        }
    }
}