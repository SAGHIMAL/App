using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace TravelBuddy.Experiencias
{
    public class ExperienciaDTO: AuditedEntityDto<Guid>
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Guid CityID { get; set; }
        public DateTime FechaExperiencia { get; set; }

        public TipoValoracion Valoracion { get; set; }
    }
}
