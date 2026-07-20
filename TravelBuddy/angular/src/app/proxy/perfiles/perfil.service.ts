import type { PerfilPublicoDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PerfilService {
  apiName = 'Default';
  

  eliminarMiCuenta = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/perfil/eliminar-mi-cuenta',
    },
    { apiName: this.apiName,...config });
  

  getPerfilPublico = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PerfilPublicoDTO>({
      method: 'GET',
      url: `/api/app/perfil/perfil-publico/${userId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
