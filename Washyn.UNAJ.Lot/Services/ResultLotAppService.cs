using Acme.BookStore.Entities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Repositories;
using Washyn.UNAJ.Lot.Controllers;

namespace Washyn.UNAJ.Lot.Services
{
    public class ResultLotAppService : ApplicationService
    {
        private readonly ILotResultRepository lotResultRepository;

        public ResultLotAppService(ILotResultRepository lotResultRepository)
        {
            this.lotResultRepository = lotResultRepository;
        }

        public async Task<List<DocenteWithRolDto>> GetAlreadyWithLot(Guid comisionId)
        {
            return await lotResultRepository.GetAlreadyWithLot(comisionId);
        }

        public async Task<List<DocenteWithRolDto>> GetWithoutLot(Guid comisionId)
        {
            return await lotResultRepository.GetWithoutLot(comisionId);
        }

        public async Task DeleteByDocenteIdAsync(Guid docenteId)
        {
            var element = await lotResultRepository.GetAsync(a => a.DocenteId == docenteId);
            await lotResultRepository.DeleteAsync(element);
        }

        public async Task DeleteLotAsync(RemoveLotResultDto model)
        {
            var element =
                await lotResultRepository.GetAsync(a => a.DocenteId == model.DocenteId && a.RolId == model.RoleId);
            await lotResultRepository.DeleteAsync(element);
        }
        
        public async Task CreateLotAsync(CreateLotResultDto create)
        {
            // validar que el docente ya este sorteado para evitar que tenga 2 roles...
            var docenteAlreadyExists = await lotResultRepository.AnyAsync(a => a.DocenteId == create.DocenteId);
            if (docenteAlreadyExists)
            {
                throw new UserFriendlyException("El docente ya esta sorteado.");
            }

            var exits = await lotResultRepository
                .AnyAsync(a => a.DocenteId == create.DocenteId && a.RolId == create.RoleId);

            if (exits)
            {
                throw new UserFriendlyException("Ya existe un rol registrado para este participante.");
            }

            await lotResultRepository.InsertAsync(new Sorteo
            {
                RolId = create.RoleId,
                DocenteId = create.DocenteId
            });
        }

        public async Task<PagedResultDto<DocenteRoleData>> GetListAsync(ResultLotFilterDto input)
        {
            var totalCount = await lotResultRepository.GetCountAsync(input.Filter);
            var data = await lotResultRepository.GetPagedListAsync(input.Filter, input.SkipCount, input.MaxResultCount,
                input.Sorting);
            return new PagedResultDto<DocenteRoleData>(totalCount, data);
        }
    }

    public class ResultLotFilterDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
    }

    public class CreateLotResultDto
    {
        public Guid DocenteId { get; set; }
        public Guid RoleId { get; set; }
    }

    public class RemoveLotResultDto
    {
        public Guid DocenteId { get; set; }
        public Guid RoleId { get; set; }
    }

    
    public class ResultLotDto : FullAuditedEntityDto
    {
        public string DocenteFullName { get; set; }
        public string RolDisplay { get; set; }
    }
}