import type { CreationAuditedEntityDto, EntityDto } from '@abp/ng.core';

export interface NotificacionDTO extends CreationAuditedEntityDto<string> {
  mensaje?: string;
  leida: boolean;
  destinoId?: string;
}

export interface PreferenciaNotificacionDTO extends EntityDto<string> {
  notificarComentariosEnFavoritos: boolean;
  notificarEventosDestinos: boolean;
}
