using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Ciudades
{
    public class SearchCityInputDTO
    {
        [StringLength(100)] 
        // El ? es porque puede ser null
        public string? nombreParcial { get; set; }
        public string? paisId { get; set; } //Este paisId hace referencia al prefijo del pais (UY, AR, etc)
        public int? minPoblacion { get; set; }
    }
}
