import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ConfigStateService } from '@abp/ng.core';
import { CalificacionService, CalificacionDTO } from '../../proxy/calificaciones';
import { ComentarioService, ComentarioDTO } from '../../proxy/comentarios';

@Component({
  selector: 'app-calificaciones-destino',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './calificaciones-destino.html',
  styleUrl: './calificaciones-destino.scss'
})
export class CalificacionesDestino implements OnInit {
  @Input({ required: true }) destinoId!: string;

  readonly estrellas = [1, 2, 3, 4, 5];

  promedio: number | null = null;
  totalCalificaciones = 0;

  get promedioRedondeado(): number {
    return Math.round(this.promedio ?? 0);
  }

  miCalificacion: CalificacionDTO | null = null;
  editando = false;
  puntajeSeleccionado = 0;
  puntajeHover = 0;
  comentarioCalificacion = '';

  comentarios: ComentarioDTO[] = [];
  nuevoComentario = '';

  cargando = true;
  guardando = false;
  error = '';

  estaLogueado = false;
  private currentUserId: string | null = null;

  private calificacionService = inject(CalificacionService);
  private comentarioService = inject(ComentarioService);
  private configState = inject(ConfigStateService);

  ngOnInit(): void {
    const currentUser = this.configState.getOne('currentUser');
    this.estaLogueado = !!currentUser?.isAuthenticated;
    this.currentUserId = currentUser?.id ?? null;

    this.cargarCalificaciones();
    this.cargarComentarios();
  }


  cargarCalificaciones(): void {
    this.cargando = true;

    this.calificacionService.getPromedio(this.destinoId).subscribe({
      next: promedio => (this.promedio = promedio),
      error: () => (this.promedio = null)
    });

    this.calificacionService.getList({ destinoId: this.destinoId, maxResultCount: 1000 }).subscribe({
      next: result => {
        this.totalCalificaciones = result.totalCount ?? result.items?.length ?? 0;
        this.miCalificacion =
          result.items?.find(c => c.userID === this.currentUserId) ?? null;
        this.cargando = false;
      },
      error: () => {
        this.cargando = false;
      }
    });
  }


  seleccionarPuntaje(puntaje: number): void {
    this.puntajeSeleccionado = puntaje;
  }

  empezarEdicion(): void {
    if (!this.miCalificacion) return;
    this.editando = true;
    this.puntajeSeleccionado = this.miCalificacion.puntaje;
    this.comentarioCalificacion = this.miCalificacion.comentario ?? '';
  }

  cancelarEdicion(): void {
    this.editando = false;
    this.puntajeSeleccionado = 0;
    this.puntajeHover = 0;
    this.comentarioCalificacion = '';
    this.error = '';
  }

  guardarCalificacion(): void {
    if (this.puntajeSeleccionado < 1 || this.puntajeSeleccionado > 5) {
      this.error = 'Elegí un puntaje de 1 a 5 estrellas.';
      return;
    }

    this.guardando = true;
    this.error = '';

    const input = {
      destinoId: this.destinoId,
      puntaje: this.puntajeSeleccionado,
      comentario: this.comentarioCalificacion?.trim() || undefined
    };

    const peticion = this.miCalificacion
      ? this.calificacionService.update(this.miCalificacion.id!, input)
      : this.calificacionService.create(input);

    peticion.subscribe({
      next: () => {
        this.guardando = false;
        this.cancelarEdicion();
        this.cargarCalificaciones();
      },
      error: err => {
        this.guardando = false;
        this.error = err?.error?.error?.message || 'No se pudo guardar la calificación.';
      }
    });
  }


  eliminarCalificacion(): void {
    if (!this.miCalificacion || !confirm('¿Eliminar tu calificación de este destino?')) {
      return;
    }

    this.guardando = true;
    this.calificacionService.delete(this.miCalificacion.id!).subscribe({
      next: () => {
        this.guardando = false;
        this.miCalificacion = null;
        this.cancelarEdicion();
        this.cargarCalificaciones();
      },
      error: err => {
        this.guardando = false;
        this.error = err?.error?.error?.message || 'No se pudo eliminar la calificación.';
      }
    });
  }


  cargarComentarios(): void {
    this.comentarioService.getListPorDestino(this.destinoId).subscribe({
      next: comentarios => (this.comentarios = comentarios),
      error: () => (this.comentarios = [])
    });
  }

  publicarComentario(): void {
    const texto = this.nuevoComentario?.trim();
    if (!texto) return;

    this.guardando = true;
    this.comentarioService.create({ destinoId: this.destinoId, texto }).subscribe({
      next: () => {
        this.guardando = false;
        this.nuevoComentario = '';
        this.cargarComentarios();
      },
      error: err => {
        this.guardando = false;
        this.error = err?.error?.error?.message || 'No se pudo publicar el comentario.';
      }
    });
  }
}
