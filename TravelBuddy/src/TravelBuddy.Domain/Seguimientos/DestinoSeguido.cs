using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Seguimientos
{
    public class DestinoSeguido : AuditedAggregateRoot<Guid>, IUserOwned
    {
        public Guid DestinoId { get; private set; }
        public Guid UserId { get; set; }

        protected DestinoSeguido()
        {
        }

        public DestinoSeguido(Guid id, Guid destinoId, Guid userId) : base(id)
        {
            DestinoId = destinoId;
            UserId = userId;
        }
    }
}
