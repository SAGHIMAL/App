import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigStateService } from '@abp/ng.core';
import { CityService } from '../../proxy/ciudades/city.service';
import { CiudadDTO } from '../../proxy/ciudades/models';
import { DestinoService } from '../../proxy/destinos/destino.service';
import { CalificacionesDestino } from '../../calificaciones/calificaciones-destino/calificaciones-destino';

@Component({
  selector: 'app-ciudad-detalle',
  imports: [CommonModule, RouterModule, CalificacionesDestino],
  standalone: true,
  templateUrl: './ciudad-detalle.html',
  styleUrl: './ciudad-detalle.scss'
})
export class CiudadDetalle {
  ciudad: CiudadDTO | null = null;
  cargando: boolean = true;
  error: string = '';

  estaLogueado = false;
  destinoGuardado = false;
  guardandoDestino = false;
  errorDestino = '';

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cityService = inject(CityService);
  private destinoService = inject(DestinoService);
  private configState = inject(ConfigStateService);

  ngOnInit(): void {
    this.estaLogueado = !!this.configState.getOne('currentUser')?.isAuthenticated;

    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      const ciudadId = Number(idParam);

      if (!isNaN(ciudadId) && ciudadId > 0) {
        this.cargarDetalle(ciudadId);
      } else {
        this.mostrarError('El ID de la ciudad no es válido.');
      }
    } else {
      this.mostrarError('No se especificó ninguna ciudad.');
    }
  }

  cargarDetalle(id: number): void {
    this.cargando = true;

    this.cityService.getCityDetail(id).subscribe({
      next: (data) => {
        this.ciudad = data;
        this.cargando = false;
        this.verificarDestinoGuardado();
      },
      error: (err) => {
        console.error('Error:', err);
        this.mostrarError('No se pudo encontrar la información de la ciudad.');
      }
    });
  }

  private verificarDestinoGuardado(): void {
    if (!this.ciudad) return;

    this.destinoService.getList({ maxResultCount: 1000 }).subscribe({
      next: result => {
        this.destinoGuardado = (result.items ?? []).some(d =>
          d.ciudad?.toLowerCase() === this.ciudad?.city?.toLowerCase() &&
          d.pais?.toLowerCase() === this.ciudad?.country?.toLowerCase()
        );
      }
    });
  }

  guardarDestino(): void {
    if (!this.ciudad || this.destinoGuardado || this.guardandoDestino) return;

    this.guardandoDestino = true;
    this.errorDestino = '';

    this.destinoService.create({
      pais: this.ciudad.country ?? '',
      ciudad: this.ciudad.city ?? '',
      coordenadas: `${this.ciudad.latitud ?? 0}, ${this.ciudad.longitud ?? 0}`,
      foto: '',
      poblacion: this.ciudad.population ?? 0
    }).subscribe({
      next: () => {
        this.guardandoDestino = false;
        this.destinoGuardado = true;
      },
      error: err => {
        this.guardandoDestino = false;
        this.errorDestino = err?.error?.error?.message || 'No se pudo guardar el destino.';
      }
    });
  }

  mostrarError(mensaje: string): void {
    this.error = mensaje;
    this.cargando = false;
  }

  volver(): void {
    this.router.navigate(['/ciudades']);
  }
}