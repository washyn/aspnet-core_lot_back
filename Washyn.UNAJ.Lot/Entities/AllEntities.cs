using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
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

    public class Rol : IEntity
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public Guid ComisionId { get; set; }
        public Rol()
        {
        }
        public object?[] GetKeys()
        {
            return new object?[] { Id, ComisionId };
        }
    }

    public class RolDto : IEntityDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }

    public class Comision : Entity<Guid>
    {
        public string Nombre { get; set; }
        public ICollection<Rol> Rols { get; set; }
    }

    public class Docente : FullAuditedEntity<Guid>
    {
        public string Dni { get; set; }
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
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
        public Area? Area { get; set; }
    }

    public class DocenteWithLookup : FullAuditedEntityDto<Guid>
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
    }


    public class DocenteWithRolDto : EntityDto<Guid>
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

        public Guid? RolId { get; set; }
        public string? RolName { get; set; }
    }


    public class DocenteFilter : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
    }

    public class CreateUpdateDocenteDto : EntityDto
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public Gender Genero { get; set; }
        public Guid GradoId { get; set; }
        public Area? Area { get; set; }
    }

    public class Sorteo : IEntity, IHasDeletionTime, ISoftDelete, IHasCreationTime, IHasModificationTime, ICreationAuditedObject
    {
        public Guid DocenteId { get; set; }
        public Guid RolId { get; set; }
        public Guid ComisionId { get; set; }

        public object?[] GetKeys()
        {
            return new object[] { DocenteId, RolId, ComisionId };
        }

        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }

    public class AppSettings : IEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public object?[] GetKeys()
        {
            return new object[] { Key };
        }
    }

    public class Participante : IEntity
    {
        public Guid DocenteId { get; set; }
        public Guid ComisionId { get; set; }
        public object?[] GetKeys()
        {
            return new object?[] { DocenteId, ComisionId };
        }
    }

    public enum Gender
    {
        Unknow,
        Male,
        Female,
    }

    public enum Area
    {
        Desconocido,
        Biomedicas,
        Sociales,
        Ingenierias,
    }
}