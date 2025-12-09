import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CityService } from '../proxy/ciudades/city.service';
import { CiudadDTO, SearchCityInputDTO } from '../proxy/ciudades/models'; 

import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-ciudades',
  standalone: true, 
  imports: [CommonModule, FormsModule], 
  templateUrl: './ciudades.html',
  styleUrls: ['./ciudades.scss'] 
})


export class CiudadesComponent { 

  ciudades: CiudadDTO[] = [];
  textoBusqueda: string = '';
  cargando: boolean = false;

  private searchSubject: Subject<string> = new Subject<string>();
  private searchSubscription: Subscription;

  private readonly ciudadService = inject(CityService);

  ngOnInit(): void {
    this.searchSubscription = this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged(),
    ).subscribe((texto) => {
      this.ejecutarBusquedaReal(texto);
    });
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
  }

  onSearchChange(texto: string): void {
    this.searchSubject.next(texto);
  }

  private ejecutarBusquedaReal(texto: string): void {
    if (!texto || texto.length < 3) {
      this.ciudades = [];
      return;
    }

    this.cargando = true;
    const input: SearchCityInputDTO = { nombreParcial: texto };

    this.ciudadService.searchCities(input).subscribe({
      next: (response) => {
        this.ciudades = response;
        this.cargando = false;
      },
      error: () => {
        this.cargando = false;
      }
    });
  }
}