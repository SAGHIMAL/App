
export interface CiudadDTO {
  id: number;
  city?: string;
  country?: string;
  region?: string;
  population?: number;
  latitud?: number;
  longitud?: number;
}

export interface SearchCityInputDTO {
  nombreParcial?: string;
  paisId?: string;
  minPoblacion?: number;
}
