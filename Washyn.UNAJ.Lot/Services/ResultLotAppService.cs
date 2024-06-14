using Acme.BookStore.Entities;
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

        public async Task CreateLotAsync(CreateLotResultDto create)
        {
            await lotResultRepository.InsertAsync(new Sorteo
            {
                RolId = create.RoleId,
                DocenteId = create.DocenteId
            });
        }

        public async Task<PagedResultDto<DocenteRoleData>> GetListAsync(ResultLotFilterDto input)
        {
            var totalCount = await lotResultRepository.GetCountAsync(input.Filter);
            var data = await lotResultRepository.GetPagedListAsync(input.Filter, input.SkipCount, input.MaxResultCount, input.Sorting);
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

    public class ResultLotDto : FullAuditedEntityDto
    {
        public string DocenteFullName { get; set; }
        public string RolDisplay { get; set; }
    }
}