using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Comentarios
{
    public class Comentario : FullAuditedEntity<Guid>, IUserOwned
    {
        public Guid DestinoId { get; set; }
        public string Texto { get; set; }

        public Guid UserId { get; set; }

        protected Comentario() { }

        public Comentario(Guid id, Guid destinoId, Guid userId, string texto)
            : base(id)
        {
            DestinoId = destinoId;
            UserId = userId;
            Texto = texto;
        }

    }
}
