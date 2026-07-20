import type { AuditedEntityDto } from '@abp/ng.core';

export interface ComentarioDTO extends AuditedEntityDto<string> {
  destinoId?: string;
  texto?: string;
  userId?: string;
}

export interface CrearComentarioDTO {
  destinoId: string;
  texto: string;
}
