using System;

namespace TravelBuddy.Ciudades
{
    public class CiudadPopularDTO
    {
        public int GeoDbId { get; set; }
        public Guid IdLocal { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Region { get; set; }
        public double Promedio { get; set; }
        public int CantidadCalificaciones { get; set; }
    }
}
