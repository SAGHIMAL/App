using System;

namespace TravelBuddy.Seguimientos
{
    public class DestinoSeguidoDTO
    {
        public Guid Id { get; set; }
        public Guid DestinoId { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Coordenadas { get; set; }
        public string Foto { get; set; }
        public int Poblacion { get; set; }
        public DateTime FechaAgregado { get; set; }
        public DateTime? UltimaActualizacion { get; set; }
    }
}
