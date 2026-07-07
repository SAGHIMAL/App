using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Experiencias
{
    public class CrearExperienciaDTO
    {
        [Required]
        public Guid CityID { get; set; }

        [Required]
        [StringLength(128)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(2048)]
        public string Descripcion { get; set; }

        public DateTime FechaExperiencia { get; set; } = DateTime.Now;

        public TipoValoracion Valoracion { get; set; }
    }
}
