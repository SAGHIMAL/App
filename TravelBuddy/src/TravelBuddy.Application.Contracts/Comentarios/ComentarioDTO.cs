using System;
using Volo.Abp.Application.Dtos;

namespace TravelBuddy.Comentarios
{
    public class ComentarioDTO : AuditedEntityDto<Guid>
    {
        public Guid DestinoId { get; set; }
        public string Texto { get; set; }
        public Guid UserId { get; set; }
    }
}
