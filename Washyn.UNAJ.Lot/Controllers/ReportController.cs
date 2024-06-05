using System.Globalization;
using Acme.BookStore.Entities;
using Acme.BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
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

        public ReportController(IOptions<DocumentOptions> options, IDocenteRepository docenteRepository)
        {
            this.options = options.Value;
            this.docenteRepository = docenteRepository;
        }

        [HttpGet]
        [Route("sample")]
        public async Task<IRemoteStreamContent> GetSamplePdfReport(Guid id)
        {
            var model = await docenteRepository.GetWithDetails(id);
            var ms = new MemoryStream(GenerateDocument(model));
            return new RemoteStreamContent(ms, "Reporte ejemplo.pdf", MimeTypes.Application.Pdf);
        }


        [HttpGet]
        [Route("all-lot-result")]
        public async Task<IRemoteStreamContent> GetAllPdfReport(Guid id)
        {
            var model = await docenteRepository.GetWithDetails(id);
            var ms = new MemoryStream(GenerateAllDocuments(model));
            return new RemoteStreamContent(ms, "Reporte ejemplo masivo.pdf", MimeTypes.Application.Pdf);
        }

        private byte[] GenerateAllDocuments(DocenteWithLookup docente)
        {
            // TODO: remove un used variables...

            QuestPDF.Settings.License = LicenseType.Community;

            var culture = new CultureInfo("es-pe");
            var documentData = new DocumentModel()
            {
                Asunto = "SOLICITO AMBIENTE DEL PRIMER PISO REPOSITORIO INSTITUCIONAL Y ALMACÉN",
                // add from app settings...
                DateGenerated = DateTime.Now,
                RolName = "DIGITADOR",
            };

            var data3 = """
                        Aprovecho la oportunidad para reiterarle los sentimientos de mi mayor consideracion y estima personal.
                        """;


            var document = Document.Create(container =>
                {
                    for (int i = 0; i < 100; i++)
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

                                    var textDate = documentData.DateGenerated.ToString("D", culture);

                                    x.Item().PaddingVertical(10).Text($"Juliaca, {textDate}").AlignEnd();

                                    x.Item().PaddingBottom(10).Text($"CARTA Nº: {options.NumeroCarta}").AlignStart().Underline().Bold();

                                    x.Item().Text($"{MapGender(docente.Genero)}:").AlignStart();
                                    x.Item().Text($"{docente.GradoPrefix} {docente.FullName}").AlignStart().Bold();
                                    x.Item().PaddingBottom(10).Text($"PRESENTE.-").AlignStart().Underline();

                                    x.Item().Text($"ASUNTO:  {documentData.Asunto}").Bold();
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


                    }
                });

            return document.GeneratePdf();
        }

        private byte[] GenerateDocument(DocenteWithLookup docente)
        {
            // TODO: remove un used variables...
            QuestPDF.Settings.License = LicenseType.Community;

            var culture = new CultureInfo("es-pe");
            var documentData = new DocumentModel()
            {
                Asunto = "SOLICITO AMBIENTE DEL PRIMER PISO REPOSITORIO INSTITUCIONAL Y ALMACÉN",
                // add from app settings...
                DateGenerated = DateTime.Now,
                RolName = "DIGITADOR",
            };

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

                                var textDate = documentData.DateGenerated.ToString("D", culture);

                                x.Item().PaddingVertical(10).Text($"Juliaca, {textDate}").AlignEnd();

                                x.Item().PaddingBottom(10).Text($"CARTA Nº: {options.NumeroCarta}").AlignStart().Underline().Bold();

                                x.Item().Text($"{MapGender(docente.Genero)}:").AlignStart();
                                x.Item().Text($"{docente.GradoPrefix} {docente.FullName}").AlignStart().Bold();
                                x.Item().PaddingBottom(10).Text($"PRESENTE.-").AlignStart().Underline();

                                x.Item().Text($"ASUNTO:  {documentData.Asunto}").Bold();
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