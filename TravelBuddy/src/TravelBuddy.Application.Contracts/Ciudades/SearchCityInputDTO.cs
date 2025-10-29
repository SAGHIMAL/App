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
        [Required]
        [StringLength(100)] // Ejemplo de validación
        public string nombreParcial { get; set; }
    }
}
