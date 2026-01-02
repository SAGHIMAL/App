import type { CiudadDTO, SearchCityInputDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CityService {
  apiName = 'Default';
  

  searchCities = (input: SearchCityInputDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CiudadDTO[]>({
      method: 'POST',
      url: '/api/app/city/search-cities',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
