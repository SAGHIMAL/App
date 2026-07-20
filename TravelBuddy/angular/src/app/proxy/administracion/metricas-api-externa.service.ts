import type { MetricasApiExternaDTO } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MetricasApiExternaService {
  apiName = 'Default';
  

  getMetricas = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, MetricasApiExternaDTO>({
      method: 'GET',
      url: '/api/app/metricas-api-externa/metricas',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
