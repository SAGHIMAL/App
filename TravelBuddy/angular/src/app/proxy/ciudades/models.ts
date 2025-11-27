
export interface CiudadDTO {
  id: number;
  city?: string;
  country?: string;
  region?: string;
}

export interface SearchCityInputDTO {
  nombreParcial: string;
}
