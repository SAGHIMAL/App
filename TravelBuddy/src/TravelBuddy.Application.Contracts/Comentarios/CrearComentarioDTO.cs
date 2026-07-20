using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TravelBuddy.Comentarios
{
    public class CrearComentarioDTO
    {
        [Required]
        public Guid DestinoId { get; set; }

        [Required]
        [StringLength(500)]
        public string Texto { get; set; }
    }
}
