using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Ciudades
{
    public class Ciudad:FullAuditedAggregateRoot<Guid>
    {

        public int GeoDbId { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Region { get; set; }

        public int? Poblacion { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

        protected Ciudad()
        {
        }

        public Ciudad(Guid id, int geoDbId, string nombre, string pais, string region, int poblacion, double latitud, double longitud) : base(id)
        {
            GeoDbId = geoDbId;
            Nombre = nombre;
            Pais = pais;
            Region = region;
            Poblacion = poblacion;
            Latitud = latitud;
            Longitud = longitud;
        }





    }
}
