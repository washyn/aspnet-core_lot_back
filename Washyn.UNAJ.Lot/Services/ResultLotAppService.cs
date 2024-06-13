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

        public async Task<PagedResultDto<ResultLotDto>> GetListAsync(ResultLotFilterDto filter)
        {
            var data = new List<ResultLotDto>()
            {
                new ResultLotDto
                {
                    CreationTime = DateTime.Now,
                    DocenteFullName = "Washington Acero",
                    RolDisplay = "DIGITADOR"
                },
                new ResultLotDto
                {
                    CreationTime = DateTime.Now,
                    DocenteFullName = "Washington Acero",
                    RolDisplay = "REVISOR DE SOCIALES"
                },
                new ResultLotDto
                {
                    CreationTime = DateTime.Now,
                    DocenteFullName = "Washington Acero",
                    RolDisplay = "REVISO DE INGENIERIAS"
                },
                new ResultLotDto
                {
                    CreationTime = DateTime.Now,
                    DocenteFullName = "Washington Acero",
                    RolDisplay = "ELABORADOR"
                },
                new ResultLotDto
                {
                    CreationTime = DateTime.Now,
                    DocenteFullName = "Washington Acero",
                    RolDisplay = "REVISOR DE ESTILO"
                }
            };
            return new PagedResultDto<ResultLotDto>(data.Count, data);
        }
    }

    public class ResultLotFilterDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
    }

    public class ResultLotDto : FullAuditedEntityDto
    {
        public string DocenteFullName { get; set; }
        public string RolDisplay { get; set; }
    }
}