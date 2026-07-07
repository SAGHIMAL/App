using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Comentarios
{
    public class Comentario : FullAuditedEntity<Guid>
    {
        public Guid DestinoId { get; set; } 
        public string Texto { get; set; }   

        public int Puntaje { get; set; } 

        protected Comentario() { }

        public Comentario(Guid id, Guid destinoId, string texto, int puntaje)
            : base(id)
        {
            DestinoId = destinoId;
            Texto = texto;
            Puntaje = puntaje;
        
        }  

    }
}
