# Notes:

### TODO:

- Desarrollo Back y front de
  - Carga de docentes en excel
  - Opcion de exportar docentes en excel
- Desarrollo generacion de documento en back, de acuerdo al formato de doc, ambos formatos y configuracion
  - Formato 1
  - Formato 2
  - EN PROCESO GENERACION DE DOCUMENTO PDF en modo prueba desde la interfaz angular.
  - Desarrollo de botton de generacion de documento en front
- Desarrollo de proceso de sorteo... PENDIENTE DE DETALLAR...
  - Resultado del sorteo(que es lo que se obtiene, es lo necesario para generar el documento)
  - Pendiente por detallar, como se realizara exactamente
  - Curso ... no se sortea ...
  - Curso y aula, como extra, despies del rol...
  - Send lot result to back for generate... create saver fake...
- Implementar el interceptor de errores y luego
- Completar la ultima parte de funcionalidad de sorteo... y queda.

### DONE:

- Servicio crud de docentes back
- Servicio se selects
- Generar plantilla documento quest pdf

- Desarrollo de crud de tablas
- Desarrollo de crud
- Add fixes
- Desarrollo front angular registro docentes
  - Pantalla en blanco despues de registros de docentes, msg de success.
- Front desarrollo de vista de lista de docentes registrados en lista
- Add area, para docente,as nullable

# IMPROVEMENT

- Can be add landing page for register docetnes...
- Add http error interceptor por mientras...
- Test...

### Sample code

```csharp
using System;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplating;
using Volo.Abp.VirtualFileSystem;
using Washyn.Billing.Company;
using Washyn.Billing.Ventas;
using Washyn.Empresa;
using Washyn.Empresa.Shared.Settings;

namespace Washyn.Billing.Web.PdfGen
{
    // canve be move to app service for consume from api...
    public class ModelComprobanteGenerator : ITransientDependency
    {
        private readonly IVirtualFileProvider _virtualFileProvider;
        private readonly CompanyInformationProvider companyInformationProvider;
        private readonly IVentaRepository ventaRepository;
        private readonly ISettingManager settingManager;
        private readonly ICurrentTenant _currentTenant;
        private readonly IBlobContainer<BillingLogoContainer> _billingBlobContainer;
        private readonly ITemplateRenderer _templateRenderer;

        public ModelComprobanteGenerator(IVirtualFileProvider virtualFileProvider,
            CompanyInformationProvider companyInformationProvider,
            IVentaRepository ventaRepository,
            ISettingManager settingManager,
            ICurrentTenant currentTenant,
            IBlobContainer<BillingLogoContainer> billingBlobContainer,
            ITemplateRenderer templateRenderer)
        {
            _virtualFileProvider = virtualFileProvider;
            this.companyInformationProvider = companyInformationProvider;
            this.ventaRepository = ventaRepository;
            this.settingManager = settingManager;
            _currentTenant = currentTenant;
            _billingBlobContainer = billingBlobContainer;
            _templateRenderer = templateRenderer;
        }

        public async Task<InvoicePdfViewModel> BuildViewModel(Guid ventaId)
        {
            var tempMOdel = await ventaRepository.GetVentaWithDetails(ventaId);
            var model = new InvoicePdfViewModel();
            model.Company = await companyInformationProvider.GetCurrentCompanyInformation();
            model.Comprobante = tempMOdel;
            model.IsDemoMode = tempMOdel.IsDemoMode;

            var logoName = await settingManager.GetOrNullGlobalAsync(OtherBillingSettingDefinitionProvider.BillingLogoSettingName);
            if (_currentTenant.IsAvailable)
            {
                logoName = await settingManager.GetOrNullForCurrentTenantAsync(OtherBillingSettingDefinitionProvider.BillingLogoSettingName);
            }

            if (!string.IsNullOrEmpty(logoName))
            {
                var fileImage = await _billingBlobContainer.GetAllBytesAsync(logoName);
                if (fileImage != null)
                {
                    model.BillingCompanyLogoBase64 = Convert.ToBase64String(fileImage);
                }
            }

            model.PercentageTax = await settingManager.GetGlobalTaxValue();
            if (_currentTenant.IsAvailable)
            {
                model.PercentageTax = await settingManager.GetCurrentTenantTaxValue();
            }
            model.QrCodeBase64 = new ComprobanteDatosHash().GenerateQrCode();
            return model;
        }
    }

    public class ComprobantePdfQuestGeneratorService : ITransientDependency
    {
        private readonly ModelComprobanteGenerator _modelComprobanteGenerator;

        public ComprobantePdfQuestGeneratorService(ModelComprobanteGenerator modelComprobanteGenerator)
        {
            _modelComprobanteGenerator = modelComprobanteGenerator;
        }
        public async Task<byte[]> Generate(Guid ventaId)
        {
            var model = await _modelComprobanteGenerator.BuildViewModel(ventaId);
            QuestPDF.Settings.License = LicenseType.Community;
            var doc = new InvoiceDocumentQuestPdf(model);
            return doc.GeneratePdf();
        }
    }

    public class InvoiceDocumentQuestPdf : QuestPDF.Infrastructure.IDocument
    {
        private readonly InvoicePdfViewModel _model;
        private readonly CultureInfo _cultureInfo;

        public InvoiceDocumentQuestPdf(InvoicePdfViewModel model)
        {
            this._model = model;
            this._cultureInfo = CultureInfo.CurrentCulture;
            this._cultureInfo = new CultureInfo("es-pe");
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.DefaultTextStyle(x => x
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken3)
                        .FontFamily("Calibri")
                    );
                    page.Content().Element(ComposeContent);

                    if (_model.IsDemoMode)
                    {
                        page.Foreground()
                        .AlignCenter()
                        .AlignMiddle()
                        .Text("""
                            Comprobante
                            de
                            prueba
                            """.ToUpper(CultureInfo.InvariantCulture))
                        .FontColor(value: "#332196f3")
                        .Bold()
                        .FontSize(50);
                    }
                });
        }


        // Improvement: alinear el logo de la empresa cuando no tenga logo.
        void ComposeContent(IContainer container)
        {
            container
                .Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        row.Spacing(10);
                        if (!string.IsNullOrEmpty(_model.BillingCompanyLogoBase64))
                        {
                            row.RelativeItem(1)
                                .Image(Convert.FromBase64String(_model.BillingCompanyLogoBase64));
                        }
                        row.RelativeItem(4)
                            .Column(col =>
                            {
                                var address = $"Dirección:  {_model.Company.Direccion}";
                                var phone = $"Contacto:  {_model.Company.NumeroCelular}";
                                var bussiness = $"{_model.Company.NombreComercial} - {_model.Company.RazonSocial}";

                                col.Spacing(-3);
                                col.Item()
                                    .AlignCenter()
                                    .Text(bussiness)
                                    .Medium()
                                    .FontSize(12);

                                col.Item()
                                    .AlignCenter()
                                    .Text(phone);

                                col.Item()
                                    .AlignCenter()
                                    .Text(address);
                            });

                        row.RelativeItem(2)
                            .Text(String.Empty);
                        row.RelativeItem(3)
                            .AlignMiddle()
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Border(0.75f)
                                    .BorderColor(Colors.Grey.Lighten1)
                                    .Column(col =>
                                    {
                                        var text = $"RUC - {_model.Comprobante.Ruc}";
                                        col.Item()
                                            .AlignCenter()
                                            .Text(text);
                                        col.Item()
                                            .LineHorizontal(0.75f)
                                            .LineColor(Colors.Grey.Lighten1);
                                        col.Item()
                                            .AlignCenter()
                                            .Text("COMPROBANTE   ELECTRONICO");
                                        col.Item()
                                            .LineHorizontal(0.75f)
                                            .LineColor(Colors.Grey.Lighten1);
                                        var docId = _model.Comprobante.DocumentName;
                                        col.Item()
                                            .AlignCenter()
                                            .Text(docId);
                                    });
                            });
                    });

                    column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.White);

                    column.Item().Row(row =>
                    {
                        row.Spacing(10);
                        row.RelativeItem(3)
                            .Column(col =>
                            {
                                col.Item().Text(text =>
                                {
                                    text.Span("Cliente: ").SemiBold();
                                    text.Span(_model.Comprobante.DisplayName);
                                });
                                col.Item().Text(text =>
                                {
                                    text.Span("Documento: ").SemiBold();
                                    text.Span(_model.Comprobante.DocumentNumber);
                                });
                            });
                        row.RelativeItem(2)
                            .Column(col =>
                            {
                                col.Item().Text(text =>
                                {
                                    text.Span("Fecha emisión: ").SemiBold();
                                    text.Span(DateTime.Now.ToString("d", _cultureInfo));
                                });
                            });
                    });

                    column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.White);

                    column.Item().Element(ComposeTable);

                    column.Item().Row(row =>
                    {
                        row.Spacing(10);
                        // NOTE: iterate this and show notes of billing...
                        row.RelativeItem(2).Column(col =>
                        {
                            col.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.White);
                            var text = $"- Son: {Washyn.Billing.NumberLetter.ConvertToLetter(_model.Comprobante.TotalVenta)}";
                            col.Item().Text(text).Medium();
                        });

                        row.RelativeItem(1).Column(col =>
                        {
                            var totalPagar = $"Total pagar: {_model.Comprobante.TotalVenta.ToString("C", _cultureInfo)}";
                            var igv = $"Total IGV {_model.PercentageTax.ToString("P2", _cultureInfo)}: {_model.Comprobante.TotalTax.ToString("C", _cultureInfo)}";
                            col.Item().AlignRight().Text(igv).SemiBold();
                            col.Item().AlignRight().Text(totalPagar).SemiBold();
                        });
                    });

                    column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.White);

                    column.Item().Column(col =>
                    {
                        col.Item().Width(50).Image(Convert.FromBase64String(_model.QrCodeBase64));
                        col.Item().Text(a =>
                        {
                            a.Span("Codigo hash:").SemiBold();
                            a.Span(_model.CodeHash);
                        });
                        col.Item().Text(a =>
                        {
                            a.Span("Condicion pago:").SemiBold();
                            a.Span(_model.PaymentMethod);
                        });
                    });
                });
        }

        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold()
                // .FontSize(9)
                ;

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(15);
                    columns.RelativeColumn(8);
                    columns.RelativeColumn();
                    columns.RelativeColumn(0.5f);
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Producto").Style(headerStyle);
                    header.Cell().AlignRight().Text("P.U.").Style(headerStyle);
                    header.Cell().AlignRight().Text("C.").Style(headerStyle);
                    header.Cell().AlignRight().Text("Total").Style(headerStyle);

                    header.Cell().ColumnSpan(5).PaddingTop(2).BorderBottom(0.75F).BorderColor(Colors.Grey.Darken1);
                });

                var counter = 1;
                foreach (var item in _model.Comprobante.Items)
                {
                    table.Cell().Element(CellStyle).Text($"{counter++}");
                    table.Cell().Element(CellStyle).Text(item.Nombre);
                    table.Cell().Element(CellStyle).AlignRight().Text(item.PrecioUnitario.ToString("C", _cultureInfo));
                    table.Cell().Element(CellStyle).AlignRight().Text(item.Cantidad.ToString("N0"));
                    table.Cell().Element(CellStyle).AlignRight().Text(item.PrecioTotal.ToString("C", _cultureInfo));

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(0.75F)
                        .BorderColor(Colors.Grey.Lighten2).PaddingVertical(1);
                }
            });
        }
    }

    public class InvoicePdfViewModel
    {
        public ComprobanteWithDetailsViewData Comprobante { get; set; }
        public CompanyInformationViewData Company { get; set; }
        public string BillingCompanyLogoBase64 { get; set; }
        public string QrCodeBase64 { get; set; }
        public string CodeHash => GetRandomHash();
        public decimal PercentageTax { get; set; }

        public string PaymentMethod => "Contado";
        public bool IsDemoMode { get; set; }

        private string GetRandomHash()
        {
            var data = new byte[10];
            new Random().NextBytes(data);
            return Convert.ToBase64String(data);
        }
    }
}


```

- Ejecucion los docentes, se sortean otros roles...
- Otro rol otra seccion.
- Consumo de api y para ejhecucuion, con otros role

---

# TODO:

- Elaboracion
- Ejecucuion pendiente... credenciales.
- Generar credenciales... con datos de prueba... que se genera...
- Tabla de comisiones...categoria superior a roles.
  - La comisiones se asigna
  - Asignacion de comisiones
- Opcion de rechar el sorteo
- Agregar opcion de renombrar comisiones... en el front.
