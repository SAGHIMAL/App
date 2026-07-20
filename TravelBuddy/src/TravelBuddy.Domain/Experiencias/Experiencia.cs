using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Experiencias
{
    public class  Experiencia : FullAuditedAggregateRoot<Guid>, IUserOwned
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Guid CityID { get; set; }

        public TipoValoracion Valoracion { get; set; }
        public DateTime FechaExperiencia { get; set; }

        public Guid UserId { get; set; }

        private Experiencia() { }

        public Experiencia(Guid id, string titulo, string descripcion, Guid cityID, Guid userId, DateTime fechaExperiencia, TipoValoracion valoracion)
            : base(id)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            CityID = cityID;
            UserId = userId;
            FechaExperiencia = fechaExperiencia;
            Valoracion = valoracion;
        }

    }
}


