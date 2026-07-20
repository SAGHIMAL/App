import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MetricasApiExternaService, MetricasApiExternaDTO } from '../../proxy/administracion';

@Component({
  selector: 'app-panel-metricas',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './panel-metricas.html',
  styleUrl: './panel-metricas.scss'
})
export class PanelMetricas implements OnInit {
  metricas: MetricasApiExternaDTO | null = null;
  cargando = true;
  error = '';

  private metricasService = inject(MetricasApiExternaService);

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.cargando = true;
    this.error = '';

    this.metricasService.getMetricas().subscribe({
      next: metricas => {
        this.metricas = metricas;
        this.cargando = false;
      },
      error: err => {
        this.error = err?.error?.error?.message || 'No se pudieron cargar las métricas.';
        this.cargando = false;
      }
    });
  }

  get porcentajeExito(): number {
    if (!this.metricas || this.metricas.totalLlamadas === 0) return 0;
    return (this.metricas.llamadasExitosas / this.metricas.totalLlamadas) * 100;
  }
}
