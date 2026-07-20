using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TravelBuddy.ApisExternas;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TravelBuddy.Administracion
{
    [Authorize(Roles = "admin")]
    public class MetricasApiExternaAppService : ApplicationService, IMetricasApiExternaAppService
    {
        private readonly IRepository<RegistroLlamadaApi, Guid> _registroLlamadaRepository;

        public MetricasApiExternaAppService(IRepository<RegistroLlamadaApi, Guid> registroLlamadaRepository)
        {
            _registroLlamadaRepository = registroLlamadaRepository;
        }

        public async Task<MetricasApiExternaDTO> GetMetricasAsync()
        {
            if (!CurrentUser.IsInRole("admin"))
            {
                throw new Volo.Abp.Authorization.AbpAuthorizationException("Solo los administradores pueden consultar las métricas.");
            }

            var registros = await _registroLlamadaRepository.GetListAsync();

            return new MetricasApiExternaDTO
            {
                TotalLlamadas = registros.Count,
                LlamadasExitosas = registros.Count(r => r.Exitosa),
                LlamadasFallidas = registros.Count(r => !r.Exitosa),
                UltimaLlamada = registros.Count > 0 ? registros.Max(r => r.CreationTime) : null,
                PorEndpoint = registros
                    .GroupBy(r => r.Endpoint)
                    .Select(g => new MetricaEndpointDTO { Endpoint = g.Key, Cantidad = g.Count() })
                    .OrderByDescending(m => m.Cantidad)
                    .ToList()
            };
        }
    }
}
