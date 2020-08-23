using AutoMapper;
using HungryPizza.Domain.Entities;
using HungryPizza.Domain.Queries.Pizza;
using HungryPizza.Domain.Queries.Request;
using HungryPizza.Infra.Shared.Models;
using HungryPizza.WebAPI.ViewModels.Queries.Pizza;
using HungryPizza.WebAPI.ViewModels.Queries.Request;

namespace HungryPizza.WebAPI.Mappers
{
    public class ViewModelToQueryMappingProfile : Profile
    {
        public ViewModelToQueryMappingProfile(AppSettings appSettings)
        {
            #region Pizza
            CreateMap<PizzaGetQueryVM, Pizza>()
                .ForMember(m => m.Id, n => n.MapFrom(f => (int) f.IdPizza));
            CreateMap<PizzaGetQueryVM, PizzaGetQuery>()
            .ForCtorParam("appSettings", p => p.MapFrom(f => appSettings));
            #endregion

            #region Request
            CreateMap<RequestGetQueryVM, RequestGetQuery>()
            .ForCtorParam("appSettings", p => p.MapFrom(f => appSettings));
            CreateMap<RequestHistoryQueryVM, RequestHistoryQuery>()
            .ForCtorParam("appSettings", p => p.MapFrom(f => appSettings));
            #endregion
        }
    }
}
