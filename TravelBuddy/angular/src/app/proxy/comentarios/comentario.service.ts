import type { ComentarioDTO, CrearComentarioDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ComentarioService {
  apiName = 'Default';
  

  create = (input: CrearComentarioDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/comentario',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getListPorDestino = (destinoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ComentarioDTO[]>({
      method: 'GET',
      url: `/api/app/comentario/por-destino/${destinoId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
