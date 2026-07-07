using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Favoritos
{
    public class Favorito : CreationAuditedEntity<Guid> 
    {
       public Guid DestinoId { get; set; }
       public Guid UserId { get; set; }

        private Favorito() { }

        public Favorito(Guid id, Guid destinoId,  Guid userId) : base(id)
        {
            DestinoId = destinoId;
            UserId = userId;
        }


    }
}
