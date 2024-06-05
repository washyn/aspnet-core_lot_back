using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Entities
{
    public class Curso : Entity<Guid>
    {
        public string Nombre { get; set; }
    }

    public class Grado : Entity<Guid>
    {
        public string Nombre { get; set; }
        public string Prefix { get; set; }
    }

    public class Rol : Entity<Guid>
    {
        public string Nombre { get; set; }
    }

    public class Docente : FullAuditedEntity<Guid>
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
        public Grado Grado { get; set; }
        public Area? Area { get; set; }
    }

    public class DocenteDto : FullAuditedEntityDto<Guid>
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
        public Area? Area { get; set; }
    }

    public class DocenteWithLookup : EntityDto<Guid>
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
        public string GradoName { get; set; }
        public string GradoPrefix { get; set; }
        public Area? Area { get; set; }

        public string FullName => Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno;
    }


    public class CreateUpdateDocenteDto : EntityDto
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
    }

    // for a single process... que es lo que se sortea...
    // como...
    public class Sorteo : FullAuditedEntity<Guid>
    {
        public Guid DocenteId { get; set; }
        public Guid RolId { get; set; }
    }

    public class Participante : Entity<Guid>
    {
        public Guid DocenteId { get; set; }
        public Guid CursoId { get; set; }
    }

    public enum Gender
    {
        Unknow,
        Male,
        Female,
    }

    public enum Area
    {
        Biomedicas,
        Sociales,
        Desconocido,
    }
}