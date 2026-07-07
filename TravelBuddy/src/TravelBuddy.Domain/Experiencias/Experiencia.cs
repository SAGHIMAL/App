using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Experiencias
{
    public class  Experiencia : FullAuditedAggregateRoot<Guid> 
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; } 
        public Guid CityID { get; set; }    //es el ID de la ciudad en la base de datos, no de la API 

        public TipoValoracion Valoracion { get; set; }
        public DateTime FechaExperiencia { get; set; }

        private Experiencia() { }

        public Experiencia(Guid id, string titulo, string descripcion, Guid cityID, DateTime fechaExperiencia, TipoValoracion valoracion)
            : base(id)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            CityID = cityID;
            FechaExperiencia = fechaExperiencia;
            Valoracion = valoracion; 
        }

    }
}


