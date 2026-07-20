import type { CreationAuditedEntityDto } from '@abp/ng.core';

export interface FavoritoDTO extends CreationAuditedEntityDto<string> {
  destinoId?: string;
  userId?: string;
}
