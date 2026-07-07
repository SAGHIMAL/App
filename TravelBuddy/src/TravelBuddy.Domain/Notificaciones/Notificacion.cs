using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Notificaciones
{
    public class Notificacion : CreationAuditedEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string Mensaje { get; set; }
        public bool Leida { get; set; }

        public Guid? DestinoId { get; set; }

        private Notificacion() { }

        public Notificacion(Guid id, Guid userId, string mensaje, Guid? destinoId = null)
            : base(id)
        {
            UserId = userId;
            Mensaje = mensaje;
            Leida = false;
            DestinoId = destinoId;
        }


    }
}
