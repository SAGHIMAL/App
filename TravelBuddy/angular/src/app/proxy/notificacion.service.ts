import type { NotificacionDTO, PreferenciaNotificacionDTO } from './notificaciones/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NotificacionService {
  apiName = 'Default';
  

  crearNotificacionPrueba = (mensaje: string, destinoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/notificacion/crear-notificacion-prueba/${destinoId}`,
      params: { mensaje },
    },
    { apiName: this.apiName,...config });
  

  getMisPreferencias = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PreferenciaNotificacionDTO>({
      method: 'GET',
      url: '/api/app/notificacion/mis-preferencias',
    },
    { apiName: this.apiName,...config });
  

  getNotificacionesASync = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, NotificacionDTO[]>({
      method: 'GET',
      url: '/api/app/notificacion/notificaciones-aSync',
    },
    { apiName: this.apiName,...config });
  

  marcarComoLeida = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/notificacion/${id}/marcar-como-leida`,
    },
    { apiName: this.apiName,...config });
  

  marcarComoNoLeida = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/notificacion/${id}/marcar-como-no-leida`,
    },
    { apiName: this.apiName,...config });
  

  updateMisPreferencias = (input: PreferenciaNotificacionDTO, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/app/notificacion/mis-preferencias',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
