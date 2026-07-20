using System;
using System.Collections.Generic;

namespace TravelBuddy.Administracion
{
    public class MetricasApiExternaDTO
    {
        public int TotalLlamadas { get; set; }
        public int LlamadasExitosas { get; set; }
        public int LlamadasFallidas { get; set; }
        public DateTime? UltimaLlamada { get; set; }
        public List<MetricaEndpointDTO> PorEndpoint { get; set; } = new();
    }

    public class MetricaEndpointDTO
    {
        public string Endpoint { get; set; }
        public int Cantidad { get; set; }
    }
}
