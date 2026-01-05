import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';
import { CityService } from '../../proxy/ciudades/city.service';
import { CiudadDTO } from '../../proxy/ciudades/models';

@Component({
  selector: 'app-ciudad-detalle',
  imports: [CommonModule, RouterModule],
  standalone: true,
  templateUrl: './ciudad-detalle.html',
  styleUrl: './ciudad-detalle.scss'
})
export class CiudadDetalle {
  ciudad: CiudadDTO | null = null;
  cargando: boolean = true;
  error: string = '';

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cityService = inject(CityService);

  ngOnInit(): void {
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
      },
      error: (err) => {
        console.error('Error:', err);
        this.mostrarError('No se pudo encontrar la información de la ciudad.');
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