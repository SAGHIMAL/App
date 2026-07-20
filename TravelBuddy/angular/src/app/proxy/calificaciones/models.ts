import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CalificacionDTO extends AuditedEntityDto<string> {
  destinoId?: string;
  puntaje: number;
  comentario?: string;
  userID?: string;
}

export interface GetCalificacionesInput extends PagedAndSortedResultRequestDto {
  destinoId?: string;
}

export interface crearCalificacionDTO {
  destinoId: string;
  puntaje: number;
  comentario?: string;
}
