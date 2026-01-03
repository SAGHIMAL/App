import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CiudadesComponent } from './ciudades';
import { CityService } from '../proxy/ciudades/city.service'; 
import { of } from 'rxjs';

describe('CiudadesComponent', () => {
  let component: CiudadesComponent;
  let fixture: ComponentFixture<CiudadesComponent>;

  const cityServiceMock = {
    searchCities: () => of([])
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CiudadesComponent],
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