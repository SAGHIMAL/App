using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace TravelBuddy.Favoritos
{
    public class FavoritoDTO : CreationAuditedEntityDto<Guid>   
    {
        public Guid DestinoId { get; set; }
        public Guid UserId { get; set; }
    }
}
