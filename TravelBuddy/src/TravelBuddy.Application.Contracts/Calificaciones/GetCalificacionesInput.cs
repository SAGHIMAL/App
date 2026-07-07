using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;    

namespace TravelBuddy.Calificaciones
{
    public class GetCalificacionesInput : PagedAndSortedResultRequestDto    
    {
        public Guid? DestinoId { get; set; }        
    }
}
