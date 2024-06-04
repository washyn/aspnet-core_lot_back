using Washyn.UNAJ.Lot.Localization;
using Volo.Abp.Application.Services;

namespace Washyn.UNAJ.Lot.Services;

/* Inherit your application services from this class. */
public abstract class LotAppService : ApplicationService
{
    protected LotAppService()
    {
        LocalizationResource = typeof(LotResource);
    }
}