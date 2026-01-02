import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CiudadesComponent } from './ciudades';
import { CityService } from '../proxy/ciudades/city.service'; 
import { of } from 'rxjs';

describe('CiudadesComponent', () => {
  let component: CiudadesComponent;
  let fixture: ComponentFixture<CiudadesComponent>;

  // Creamos un "doble" (mock) del servicio para que la prueba no intente llamar al backend real
  const cityServiceMock = {
    searchCities: () => of([]) // Devuelve un observable vacío simulado
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // Importamos el componente (porque es standalone)
      imports: [CiudadesComponent],
      // Proveemos el mock en lugar del servicio real
      providers: [
        { provide: CityService, useValue: cityServiceMock }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CiudadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});