
export interface MetricaEndpointDTO {
  endpoint?: string;
  cantidad: number;
}

export interface MetricasApiExternaDTO {
  totalLlamadas: number;
  llamadasExitosas: number;
  llamadasFallidas: number;
  ultimaLlamada?: string;
  porEndpoint: MetricaEndpointDTO[];
}
