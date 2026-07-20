using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace TravelBuddy.Notificaciones
{
    public class PreferenciaNotificacion : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public bool NotificarComentariosEnFavoritos { get; set; } = true;
        public bool NotificarEventosDestinos { get; set; } = true;

        protected PreferenciaNotificacion() { } 

        public PreferenciaNotificacion(Guid id, Guid userId) : base(id)
        {
            UserId = userId;
            NotificarComentariosEnFavoritos = true;
            NotificarEventosDestinos = true;
        }
    }
}
