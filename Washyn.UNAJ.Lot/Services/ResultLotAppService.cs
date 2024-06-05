using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities.Auditing;

namespace Washyn.UNAJ.Lot.Services
{
    public class ResultLotAppService : ApplicationService
    {
        public ResultLotAppService()
        {
        }

        public async Task<PagedResultDto<ResultLotDto>> GetListAsync()
        {
            var data = new List<ResultLotDto>()
            {
                new ResultLotDto
                {
                    CreationTime = DateTime.Now,
                    DocenteFullName = "Washington Acero",
                    RolDisplay = "Digitador"
                }
            };
            return new PagedResultDto<ResultLotDto>(data.Count, data);
        }
    }

    public class ResultLotDto : FullAuditedEntityDto
    {
        public string DocenteFullName { get; set; }
        public string RolDisplay { get; set; }
    }
}