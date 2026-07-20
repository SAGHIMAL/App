import type { FavoritoDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FavoritoService {
  apiName = 'Default';
  

  create = (destinoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/favorito',
      params: { destinoId },
    },
    { apiName: this.apiName,...config });
  

  delete = (destinoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/favorito',
      params: { destinoId },
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FavoritoDTO[]>({
      method: 'GET',
      url: '/api/app/favorito',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
