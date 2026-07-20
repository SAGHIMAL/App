import type { CrearExperienciaDTO, ExperienciaDTO, GetExperienciasInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ExperienciaService {
  apiName = 'Default';
  

  create = (input: CrearExperienciaDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExperienciaDTO>({
      method: 'POST',
      url: '/api/app/experiencia',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/experiencia/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExperienciaDTO>({
      method: 'GET',
      url: `/api/app/experiencia/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetExperienciasInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ExperienciaDTO>>({
      method: 'GET',
      url: '/api/app/experiencia',
      params: { textoFiltrado: input.textoFiltrado, cityID: input.cityID, valoracion: input.valoracion, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CrearExperienciaDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExperienciaDTO>({
      method: 'PUT',
      url: `/api/app/experiencia/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
