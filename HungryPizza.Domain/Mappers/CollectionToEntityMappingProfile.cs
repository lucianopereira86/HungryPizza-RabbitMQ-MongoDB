using AutoMapper;
using HungryPizza.Domain.Entities;
using HungryPizza.Domain.Entities.History;
using HungryPizza.Infra.Data.MongoDB.Collections;
using HungryPizza.Infra.Data.MongoDB.Collections.History;

namespace HungryPizza.Domain.Mappers
{
    public class CollectionToEntityMappingProfile : Profile
    {
        public CollectionToEntityMappingProfile()
        {
            #region Request
            CreateMap<PizzaCollection, Pizza>();
            CreateMap<RequestCollection, Request>();
            CreateMap<RequestPizzaCollection, RequestPizza>();
            CreateMap<RequestHistoryCollection, RequestHistory>();
            CreateMap<RequestHistoryPizzaCollection, RequestHistoryPizza>();
            CreateMap<RequestHistoryPizzaInfoCollection, RequestHistoryPizzaInfo>();
            #endregion
        }
    }
}
