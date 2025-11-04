using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;
using Volo.Abp.Domain.Entities;
using Volo.Abp;


namespace TravelBuddy.Calificaciones
{
    public class Calificacion : AuditedAggregateRoot<Guid>, IUserOwned

    {
        // 1. Propiedades de la entidad
        public Guid DestinoId { get; private set; }
        public int Puntaje { get; private set; }

        public string Comentario { get; private set; }
        // 2. Propiedad de la interfaz IUserOwned
        public Guid UserId { get; set; }

        // 3. Constructor
        private Calificacion()
        {
            /* Este constructor es requerido por Entity Framework Core. */
        }

        public Calificacion(
            Guid id,
            Guid destinoId,
            Guid userId,
            int puntaje,
            string comentario) : base(id)
        {
            DestinoId = destinoId;
            UserId = userId;
            Puntaje = puntaje; // Aquí podrías agregar validaciones (ej. que esté entre 1 y 5)
            Comentario = comentario;
        }
    }


}