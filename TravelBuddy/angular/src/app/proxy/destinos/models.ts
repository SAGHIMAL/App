import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdatedestinoDTO {
  pais: string;
  ciudad: string;
  coordenadas?: string;
  foto?: string;
  poblacion: number;
}

export interface destinoDTO extends AuditedEntityDto<string> {
  id?: string;
  ciudad?: string;
  coordenadas?: string;
  pais?: string;
  foto?: string;
  poblacion: number;
}
