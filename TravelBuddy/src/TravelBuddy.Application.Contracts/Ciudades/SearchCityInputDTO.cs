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
        public string? nombreParcial { get; set; }
        public string? paisId { get; set; }
        public int? minPoblacion { get; set; }
    }
}
