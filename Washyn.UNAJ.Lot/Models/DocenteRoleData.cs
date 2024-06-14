using Acme.BookStore.Entities;
using Volo.Abp.Application.Dtos;
namespace Washyn.UNAJ.Lot.Controllers
{
    public class DocenteRoleData : FullAuditedEntityDto<Guid>
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
        public string GradoName { get; set; }
        public string GradoPrefix { get; set; }
        public Area? Area { get; set; }

        public string FullName { get; set; }
        public string RolName { get; set; }
    }
}