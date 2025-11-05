using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;
using Volo.Abp.Domain.Entities;
using Volo.Abp;


namespace TravelBuddy.Calificaciones
{
    public class Calificacion : AuditedAggregateRoot<Guid>, IUserOwned

    {
   
        public Guid DestinoId { get; private set; }
        public int Puntaje { get; private set; }

        public string? Comentario { get; private set; }
   
        public Guid UserId { get; set; }

  
        private Calificacion()
        {
           
        }

        public Calificacion(
            Guid id,
            Guid destinoId,
            Guid userId,
            int puntaje,
            string? comentario) : base(id)
        {
            if (puntaje < 1 || puntaje > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(puntaje), "El puntaje debe estar entre 1 y 5.");
            }
            DestinoId = destinoId;
            UserId = userId;
            Puntaje = puntaje; 
            Comentario = comentario;
        }
    }


}