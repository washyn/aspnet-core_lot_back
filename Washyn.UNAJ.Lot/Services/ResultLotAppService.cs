using System.ComponentModel.DataAnnotations;
using Acme.BookStore.Entities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Washyn.UNAJ.Lot.Controllers;

namespace Washyn.UNAJ.Lot.Services
{
    /// <summary>
    /// Servicio para administgrar sorteos.
    /// </summary>
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
        
        public async Task DeleteLotAsync(RemoveLotResultDto model)
        {
            var element =
                await lotResultRepository.GetAsync(a => a.DocenteId == model.DocenteId && a.RolId == model.RoleId && a.ComisionId == model.ComisionId);
            await lotResultRepository.DeleteAsync(element);
        }

        public async Task CreateLotAsync(CreateLotResultDto create)
        {
            var exits = await lotResultRepository
                .AnyAsync(a => a.DocenteId == create.DocenteId
                    && a.RolId == create.RoleId
                    && a.ComisionId == create.ComisionId);

            if (exits)
            {
                throw new UserFriendlyException("Este participante ya esta registrado.");
            }

            await lotResultRepository.InsertAsync(new Sorteo
            {
                RolId = create.RoleId,
                DocenteId = create.DocenteId,
                ComisionId = create.ComisionId,
            });
        }

        /// <summary>
        /// Retorna la lista de docentes sorteados para la vista y para el reporte pdf.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        [Required]
        public Guid DocenteId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid ComisionId { get; set; }
    }

    public class RemoveLotResultDto
    {
        [Required]
        public Guid DocenteId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Guid ComisionId { get; set; }
    }
}