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

    public class Rol : Entity<Guid>
    {
        public string Nombre { get; set; }
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
    // TODO: evitar duplicados... add pk composite...
    // TODO: regen db
    // for a single process... que es lo que se sortea...
    // como...
    public class Sorteo : IEntity, IHasDeletionTime, ISoftDelete, IHasCreationTime, IHasModificationTime, ICreationAuditedObject
    {
        public Guid DocenteId { get; set; }
        public Guid RolId { get; set; }

        public object?[] GetKeys()
        {
            return new object[] { DocenteId, RolId };
        }

        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }

        // public override Guid? CreatorId { get => base.CreatorId; protected set => base.CreatorId = value; }
        // public override DateTime CreationTime { get => base.CreationTime; protected set => base.CreationTime = value; }
        // public override Guid? DeleterId { get => base.DeleterId; set => base.DeleterId = value; }
        // public override DateTime? DeletionTime { get => base.DeletionTime; set => base.DeletionTime = value; }
        // public override Guid? LastModifierId { get => base.LastModifierId; set => base.LastModifierId = value; }
        // public override bool IsDeleted { get => base.IsDeleted; set => base.IsDeleted = value; }
        // public override DateTime? LastModificationTime { get => base.LastModificationTime; set => base.LastModificationTime = value; }
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
        Desconocido,
        Biomedicas,
        Sociales,
        Ingenierias,
    }
}