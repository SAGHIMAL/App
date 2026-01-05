import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CityService } from '../proxy/ciudades/city.service';
import { CiudadDTO, SearchCityInputDTO } from '../proxy/ciudades/models'; 
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-ciudades',
  standalone: true, 
  imports: [CommonModule, FormsModule, RouterModule], 
  templateUrl: './ciudades.html',
  styleUrls: ['./ciudades.scss'] 
})
export class CiudadesComponent { 

  ciudades: CiudadDTO[] = [];
  textoBusqueda: string = '';
  cargando: boolean = false;

  filtroPoblacion: number | null = null;
  filtroPais: string = ''; 

  private searchSubject: Subject<void> = new Subject<void>();
  private searchSubscription: Subscription;

  private readonly ciudadService = inject(CityService);

  ngOnInit(): void {
    this.searchSubscription = this.searchSubject.pipe(
      debounceTime(500)
    ).subscribe(() => {
      this.ejecutarBusquedaReal();
    });
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
  }

  onSearchChange(): void {
    this.searchSubject.next();
  }

  private ejecutarBusquedaReal(): void {
    
    const tieneTextoValido = this.textoBusqueda && this.textoBusqueda.length >= 2;
    const tieneFiltros = (this.filtroPais && this.filtroPais.length > 0) || (this.filtroPoblacion && this.filtroPoblacion > 0);

    if (!tieneTextoValido && !tieneFiltros) {
        if (!this.textoBusqueda && !tieneFiltros) {
            this.ciudades = [];
        }
        return;
    }

    this.cargando = true;

    const input: SearchCityInputDTO = { 
      nombreParcial: this.textoBusqueda || undefined,
      paisId: this.filtroPais || undefined,
      minPoblacion: this.filtroPoblacion || undefined,
    };

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