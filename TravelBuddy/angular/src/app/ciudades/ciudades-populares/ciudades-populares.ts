import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CityService } from '../../proxy/ciudades/city.service';
import { CiudadPopularDTO } from '../../proxy/ciudades/models';

@Component({
  selector: 'app-ciudades-populares',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './ciudades-populares.html',
  styleUrl: './ciudades-populares.scss'
})
export class CiudadesPopulares implements OnInit {
  readonly estrellas = [1, 2, 3, 4, 5];

  populares: CiudadPopularDTO[] = [];
  cargando = true;
  error = '';

  private cityService = inject(CityService);

  ngOnInit(): void {
    this.cityService.getCiudadesPopulares(10).subscribe({
      next: populares => {
        this.populares = populares;
        this.cargando = false;
      },
      error: () => {
        this.error = 'No se pudieron cargar los destinos populares.';
        this.cargando = false;
      }
    });
  }

  redondear(promedio: number): number {
    return Math.round(promedio);
  }
}
