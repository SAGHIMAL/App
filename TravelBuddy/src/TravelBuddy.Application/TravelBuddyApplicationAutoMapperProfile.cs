using AutoMapper;
using TravelBuddy.Destinos;

using Volo.Abp.Domain.Entities.Events.Distributed;

namespace TravelBuddy;

public class TravelBuddyApplicationAutoMapperProfile : Profile
{
    public TravelBuddyApplicationAutoMapperProfile()
    {

        CreateMap < Destino, destinoDTO >();  
        CreateMap < CreateUpdatedestinoDTO, Destino >();    
    }
}
