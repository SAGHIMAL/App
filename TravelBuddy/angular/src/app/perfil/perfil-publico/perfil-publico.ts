import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PerfilService, PerfilPublicoDTO } from '../../proxy/perfiles';
import { TipoValoracion } from '../../proxy/experiencias';

@Component({
  selector: 'app-perfil-publico',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './perfil-publico.html',
  styleUrl: './perfil-publico.scss'
})
export class PerfilPublico implements OnInit {
  perfil: PerfilPublicoDTO | null = null;
  cargando = true;
  error = '';

  readonly TipoValoracion = TipoValoracion;

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private perfilService = inject(PerfilService);

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('userId');
    if (!userId) {
      this.mostrarError('No se especificó ningún usuario.');
      return;
    }

    this.perfilService.getPerfilPublico(userId).subscribe({
      next: perfil => {
        this.perfil = perfil;
        this.cargando = false;
      },
      error: err => {
        this.mostrarError(err?.error?.error?.message || 'No se pudo cargar el perfil.');
      }
    });
  }

  get nombreCompleto(): string {
    const partes = [this.perfil?.nombre, this.perfil?.apellido].filter(Boolean);
    return partes.length ? partes.join(' ') : this.perfil?.userName ?? '';
  }

  valoracionTexto(valoracion?: TipoValoracion): string {
    switch (valoracion) {
      case TipoValoracion.Positiva: return 'Positiva';
      case TipoValoracion.Neutral: return 'Neutral';
      case TipoValoracion.Negativa: return 'Negativa';
      default: return '';
    }
  }

  valoracionClase(valoracion?: TipoValoracion): string {
    switch (valoracion) {
      case TipoValoracion.Positiva: return 'bg-success';
      case TipoValoracion.Neutral: return 'bg-secondary';
      case TipoValoracion.Negativa: return 'bg-danger';
      default: return 'bg-light text-dark';
    }
  }

  mostrarError(mensaje: string): void {
    this.error = mensaje;
    this.cargando = false;
  }

  volver(): void {
    this.router.navigate(['/']);
  }
}
