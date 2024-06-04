using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

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
    
    public class Docente : Entity<Guid>
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
        public Grado Grado { get; set; }
    }
    
    public class DocenteDto : EntityDto<Guid>
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
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
    public class Sorteo : Entity<Guid>
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
}