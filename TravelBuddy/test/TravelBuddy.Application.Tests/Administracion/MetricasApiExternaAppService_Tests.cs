using Shouldly;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBuddy.Administracion;
using TravelBuddy.ApisExternas;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace TravelBuddy.Administracion
{
    public abstract class MetricasApiExternaAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IMetricasApiExternaAppService _metricasAppService;
        private readonly IRepository<RegistroLlamadaApi, Guid> _registroRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public MetricasApiExternaAppService_Tests()
        {
            _metricasAppService = GetRequiredService<IMetricasApiExternaAppService>();
            _registroRepository = GetRequiredService<IRepository<RegistroLlamadaApi, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        private static ClaimsPrincipal CrearPrincipal(Guid userId, params string[] roles)
        {
            var claims = new System.Collections.Generic.List<Claim>
            {
                new Claim(AbpClaimTypes.UserName, "testuser-metricas"),
                new Claim(AbpClaimTypes.UserId, userId.ToString())
            };
            claims.AddRange(roles.Select(r => new Claim(AbpClaimTypes.Role, r)));

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        [Fact]
        public async Task Should_Devolver_Metricas_Al_Admin()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                await _registroRepository.InsertAsync(new RegistroLlamadaApi(Guid.NewGuid(), "GeoDb", "cities (busqueda)", exitosa: true));
                await _registroRepository.InsertAsync(new RegistroLlamadaApi(Guid.NewGuid(), "GeoDb", "cities (busqueda)", exitosa: true));
                await _registroRepository.InsertAsync(new RegistroLlamadaApi(Guid.NewGuid(), "GeoDb", "cities/{id} (detalle)", exitosa: false));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid(), "admin")))
            {
                var metricas = await _metricasAppService.GetMetricasAsync();

                metricas.TotalLlamadas.ShouldBe(3);
                metricas.LlamadasExitosas.ShouldBe(2);
                metricas.LlamadasFallidas.ShouldBe(1);
                metricas.UltimaLlamada.ShouldNotBeNull();
                metricas.PorEndpoint.Count.ShouldBe(2);
                metricas.PorEndpoint.First().Endpoint.ShouldBe("cities (busqueda)");
                metricas.PorEndpoint.First().Cantidad.ShouldBe(2);
            }
        }

        [Fact]
        public async Task Should_Not_Devolver_Metricas_A_Usuario_Comun()
        {
            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid() )))
            {
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _metricasAppService.GetMetricasAsync();
                });
            }
        }

        [Fact]
        public async Task Should_Not_Devolver_Metricas_Sin_Login()
        {
            using (_currentPrincipalAccessor.Change(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _metricasAppService.GetMetricasAsync();
                });
            }
        }
    }
}
