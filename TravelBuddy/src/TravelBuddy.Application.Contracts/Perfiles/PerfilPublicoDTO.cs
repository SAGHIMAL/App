using System;
using System.Collections.Generic;
using TravelBuddy.Experiencias;

namespace TravelBuddy.Perfiles
{
    public class PerfilPublicoDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Foto { get; set; }
        public List<ExperienciaDTO> Experiencias { get; set; } = new();
    }
}
