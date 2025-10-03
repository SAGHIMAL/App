using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Application.Dtos;
using System.Threading.Tasks;

namespace TravelBuddy.Destinos;

public class destinoDTO: AuditedEntityDto<Guid>  
{
    public Guid Id { get; set; }
    public string Ciudad { get; set; }
    public string Coordenadas { get; set; }
    public string Pais { get; set; }
    public string Foto { get; set; }

    public int Poblacion { get; set; }  
}




