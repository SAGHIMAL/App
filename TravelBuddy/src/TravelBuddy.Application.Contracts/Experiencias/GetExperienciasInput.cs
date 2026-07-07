using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using TravelBuddy.Experiencias;

namespace TravelBuddy.Experiencias
{
    public class GetExperienciasInput : PagedAndSortedResultRequestDto
    {
        public string? TextoFiltrado { get; set; } 
        public Guid? CityID { get; set; }    

        public TipoValoracion? Valoracion { get; set; }  
    }
}
