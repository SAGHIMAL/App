import type { DestinoSeguidoDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SeguimientoService {
  apiName = 'Default';
  

  agregar = (destinoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/seguimiento/agregar/${destinoId}`,
    },
    { apiName: this.apiName,...config });
  

  eliminar = (destinoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/seguimiento/eliminar/${destinoId}`,
    },
    { apiName: this.apiName,...config });
  

  getMiLista = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DestinoSeguidoDTO[]>({
      method: 'GET',
      url: '/api/app/seguimiento/mi-lista',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
