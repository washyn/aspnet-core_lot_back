using Acme.BookStore.Entities;
using AutoMapper;
using Washyn.UNAJ.Lot.Services;

namespace Washyn.UNAJ.Lot.ObjectMapping;

public class LotAutoMapperProfile : Profile
{
    public LotAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */
        CreateMap<DocenteDto, Docente>().ReverseMap();
        CreateMap<Docente, CreateUpdateDocenteDto>().ReverseMap();
        CreateMap<ComisionDto, Comision>().ReverseMap();
    }
}
