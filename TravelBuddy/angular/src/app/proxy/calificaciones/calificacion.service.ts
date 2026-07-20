import type { CalificacionDTO, GetCalificacionesInput, crearCalificacionDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CalificacionService {
  apiName = 'Default';
  

  create = (input: crearCalificacionDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CalificacionDTO>({
      method: 'POST',
      url: '/api/app/calificacion',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/calificacion/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CalificacionDTO>({
      method: 'GET',
      url: `/api/app/calificacion/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetCalificacionesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CalificacionDTO>>({
      method: 'GET',
      url: '/api/app/calificacion',
      params: { destinoId: input.destinoId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPromedio = (destinoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: `/api/app/calificacion/promedio/${destinoId}`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: crearCalificacionDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CalificacionDTO>({
      method: 'PUT',
      url: `/api/app/calificacion/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
