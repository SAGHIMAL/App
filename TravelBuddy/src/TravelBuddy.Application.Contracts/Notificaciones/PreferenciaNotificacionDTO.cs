using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
namespace TravelBuddy.Notificaciones
{
    public class PreferenciaNotificacionDTO : EntityDto<Guid>
    {
        public bool NotificarComentariosEnFavoritos { get; set; }   
        public bool NotificarEventosDestinos { get; set; } 
    }
}
