using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace TravelBuddy.Calificaciones
{
    public class CalificacionDTO : AuditedEntityDto<Guid>   
    {
        public Guid DestinoId { get; set; } 
        public int Puntaje { get; set; }    
        public string? Comentario { get; set; } 

        public Guid UserID { get; set; }    
    }
}
