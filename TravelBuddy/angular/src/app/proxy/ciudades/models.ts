
export interface CiudadDTO {
  id: number;
  idLocal?: string;
  city?: string;
  country?: string;
  region?: string;
  population?: number;
  latitud?: number;
  longitud?: number;
}

export interface CiudadPopularDTO {
  geoDbId: number;
  idLocal?: string;
  nombre?: string;
  pais?: string;
  region?: string;
  promedio: number;
  cantidadCalificaciones: number;
}

export interface SearchCityInputDTO {
  nombreParcial?: string;
  paisId?: string;
  minPoblacion?: number;
}
