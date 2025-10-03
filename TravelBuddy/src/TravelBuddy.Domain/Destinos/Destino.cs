using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.Destinos;

public class Destino : AuditedAggregateRoot<Guid>
{
    public string Pais { get; set; }
    public string Ciudad { get; set; }
    public string Coordenadas { get; set; }
    public string Foto { get; set; }
    public int Poblacion { get; set; }  

    // Constructor requerido por Entity Framework
    protected Destino()
    {
    }

    public Destino(Guid id, string pais, string ciudad, string coordenadas, string foto, int Poblacion )
        : base(id)
    {
        Pais = pais;
        Ciudad = ciudad;
        Coordenadas = coordenadas;
        Foto = foto;
        Poblacion = 0;
    }
}

