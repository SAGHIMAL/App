import type { ExperienciaDTO } from '../experiencias/models';

export interface PerfilPublicoDTO {
  userId?: string;
  userName?: string;
  nombre?: string;
  apellido?: string;
  foto?: string;
  experiencias: ExperienciaDTO[];
}
