using AutoMapper;
using TravelBuddy.Destinos;
using TravelBuddy.Experiencias; 
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Events.Distributed;
using TravelBuddy.Calificaciones;
using TravelBuddy.Favoritos;
using TravelBuddy.Notificaciones;



namespace TravelBuddy;

public class TravelBuddyApplicationAutoMapperProfile : Profile
{
    public TravelBuddyApplicationAutoMapperProfile()
    {

        CreateMap < Destino, destinoDTO >();  
        CreateMap < CreateUpdatedestinoDTO, Destino >();   
        CreateMap < Experiencia, ExperienciaDTO >();
        CreateMap < CrearExperienciaDTO, Experiencia >();   
        CreateMap < Calificacion, CalificacionDTO >(); 
        CreateMap <Favorito, FavoritoDTO >();
        CreateMap < Notificacion, NotificacionDTO >();
        CreateMap<PreferenciaNotificacion, PreferenciaNotificacionDTO>();
        CreateMap<PreferenciaNotificacionDTO, PreferenciaNotificacion>();
    }
}
