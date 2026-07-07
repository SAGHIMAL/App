using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace TravelBuddy.Notificaciones
{
    public class NotificacionDTO : CreationAuditedEntityDto<Guid>
    {
        public string Mensaje { get; set; } 
        public bool Leida { get; set; } 
        public Guid? DestinoId { get; set; }    
    }
}
