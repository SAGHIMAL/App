import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '@abp/ng.core';
import { ProfileService } from '@abp/ng.account.core/proxy';
import { PerfilService } from '../../proxy/perfiles';
import { NotificacionService } from '../../proxy/notificacion.service';
import { PreferenciaNotificacionDTO } from '../../proxy/notificaciones/models';

@Component({
  selector: 'app-mi-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './mi-perfil.html',
  styleUrl: './mi-perfil.scss'
})
export class MiPerfil implements OnInit {
  userName = '';
  fotoUrl = '';
  fotoGuardada = '';

  preferencias: PreferenciaNotificacionDTO = {
    notificarComentariosEnFavoritos: true,
    notificarEventosDestinos: true
  } as PreferenciaNotificacionDTO;

  confirmacionEliminar = '';

  cargando = true;
  guardandoFoto = false;
  guardandoPreferencias = false;
  eliminando = false;
  mensaje = '';
  error = '';

  private profileService = inject(ProfileService);
  private perfilService = inject(PerfilService);
  private notificacionService = inject(NotificacionService);
  private authService = inject(AuthService);
  private router = inject(Router);

  ngOnInit(): void {
    this.profileService.get().subscribe({
      next: perfil => {
        this.userName = perfil.userName ?? '';
        this.fotoGuardada = (perfil.extraProperties?.['Foto'] as string) ?? '';
        this.fotoUrl = this.fotoGuardada;
        this.cargando = false;
      },
      error: () => {
        this.error = 'No se pudo cargar tu perfil.';
        this.cargando = false;
      }
    });

    this.notificacionService.getMisPreferencias().subscribe({
      next: prefs => (this.preferencias = prefs)
    });
  }


  guardarFoto(): void {
    this.guardandoFoto = true;
    this.mensaje = '';
    this.error = '';

    this.profileService.get().subscribe({
      next: perfil => {
        const input = {
          ...perfil,
          extraProperties: { ...perfil.extraProperties, Foto: this.fotoUrl?.trim() ?? '' }
        };
        this.profileService.update(input).subscribe({
          next: () => {
            this.guardandoFoto = false;
            this.fotoGuardada = this.fotoUrl?.trim() ?? '';
            this.mensaje = 'Foto de perfil actualizada.';
          },
          error: err => {
            this.guardandoFoto = false;
            this.error = err?.error?.error?.message || 'No se pudo guardar la foto.';
          }
        });
      },
      error: () => {
        this.guardandoFoto = false;
        this.error = 'No se pudo guardar la foto.';
      }
    });
  }


  guardarPreferencias(): void {
    this.guardandoPreferencias = true;
    this.mensaje = '';
    this.error = '';

    this.notificacionService.updateMisPreferencias(this.preferencias).subscribe({
      next: () => {
        this.guardandoPreferencias = false;
        this.mensaje = 'Preferencias de notificación guardadas.';
      },
      error: err => {
        this.guardandoPreferencias = false;
        this.error = err?.error?.error?.message || 'No se pudieron guardar las preferencias.';
      }
    });
  }


  get puedeEliminar(): boolean {
    return this.confirmacionEliminar.trim().toUpperCase() === 'ELIMINAR';
  }

  eliminarCuenta(): void {
    if (!this.puedeEliminar || this.eliminando) return;
    if (!confirm('Esta acción es permanente: se borra tu cuenta. ¿Continuar?')) return;

    this.eliminando = true;
    this.error = '';

    this.perfilService.eliminarMiCuenta().subscribe({
      next: () => {
        this.authService.logout().subscribe({
          next: () => this.router.navigate(['/']),
          error: () => this.router.navigate(['/'])
        });
      },
      error: err => {
        this.eliminando = false;
        this.error = err?.error?.error?.message || 'No se pudo eliminar la cuenta.';
      }
    });
  }
}
