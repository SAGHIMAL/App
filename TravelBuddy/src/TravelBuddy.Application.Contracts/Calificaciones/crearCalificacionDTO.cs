using System;
using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Calificaciones
{
    public class crearCalificacionDTO 
    {

       [Required]
        public Guid DestinoId { get; set; }

    
        [Required]
        [Range(1, 5)]
        public int Puntaje { get; set; }

    
        public string Comentario { get; set; }
    }
}
