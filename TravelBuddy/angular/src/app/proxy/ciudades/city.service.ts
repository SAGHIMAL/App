import type { CiudadDTO, SearchCityInputDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CityService {
  apiName = 'Default';
  

  getCityDetail = (geoDBId: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CiudadDTO>({
      method: 'GET',
      url: `/api/app/city/city-detail/${geoDBId}`,
    },
    { apiName: this.apiName,...config });
  

  searchCities = (input: SearchCityInputDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CiudadDTO[]>({
      method: 'POST',
      url: '/api/app/city/search-cities',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
