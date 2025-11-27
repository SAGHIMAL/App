import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CityService } from '../proxy/ciudades/city.service';
import { CiudadDTO, SearchCityInputDTO } from '../proxy/ciudades/models'; 

@Component({
  selector: 'app-ciudades',
  standalone: true, 
  imports: [CommonModule, FormsModule], 
  templateUrl: './ciudades.html',
  styleUrls: ['./ciudades.scss'] 
})


export class CiudadesComponent { 

  // 1. Variables para la vista
  ciudades: CiudadDTO[] = []; // La lista de resultados
  textoBusqueda: string = ''; // Lo que escribe el usuario

  cargando: boolean = false;

  // 2. Inyectamos el servicio de ABP (Backend)
  private readonly ciudadService = inject(CityService);

  // 3. Método para buscar (reemplaza al create/delete del ejemplo)
  buscar(): void {
    // Validamos que no esté vacío
    if (!this.textoBusqueda || this.textoBusqueda.length < 3) {
      return; 
    }

    this.cargando = true;

    // Creamos el DTO de entrada que pide el backend
    const input: SearchCityInputDTO = {
      nombreParcial: this.textoBusqueda
    };
    
    // Llamamos al servicio
    this.ciudadService.searchCities(input).subscribe({
      next: (response) => {
        this.ciudades = response;
        this.cargando = false;
      },
      error: (err) => {
        console.error('Error', err);
        this.cargando = false;
      }
    });
  }
}