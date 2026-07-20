import type { TipoValoracion } from './tipo-valoracion.enum';
import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CrearExperienciaDTO {
  cityID: string;
  titulo: string;
  descripcion: string;
  fechaExperiencia?: string;
  valoracion?: TipoValoracion;
}

export interface ExperienciaDTO extends AuditedEntityDto<string> {
  titulo?: string;
  descripcion?: string;
  cityID?: string;
  fechaExperiencia?: string;
  valoracion?: TipoValoracion;
  userId?: string;
}

export interface GetExperienciasInput extends PagedAndSortedResultRequestDto {
  textoFiltrado?: string;
  cityID?: string;
  valoracion?: TipoValoracion;
}
