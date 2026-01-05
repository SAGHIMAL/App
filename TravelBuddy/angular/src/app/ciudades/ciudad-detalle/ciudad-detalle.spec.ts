import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CiudadDetalle } from './ciudad-detalle';

describe('CiudadDetalle', () => {
  let component: CiudadDetalle;
  let fixture: ComponentFixture<CiudadDetalle>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CiudadDetalle]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CiudadDetalle);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
