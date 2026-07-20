using Shouldly;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBuddy.Destinos;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace TravelBuddy.Seguimientos
{
    public abstract class SeguimientoAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly ISeguimientoAppService _seguimientoAppService;
        private readonly IRepository<DestinoSeguido, Guid> _seguimientoRepository;
        private readonly IRepository<Destino, Guid> _destinoRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public SeguimientoAppService_Tests()
        {
            _seguimientoAppService = GetRequiredService<ISeguimientoAppService>();
            _seguimientoRepository = GetRequiredService<IRepository<DestinoSeguido, Guid>>();
            _destinoRepository = GetRequiredService<IRepository<Destino, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        private async Task<Guid> CrearDestinoAsync(string pais = "Francia", string ciudad = "Paris")
        {
            var destinoId = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () =>
            {
                await _destinoRepository.InsertAsync(
                    new Destino(destinoId, pais, ciudad, "48.8566° N, 2.3522° E", "https://example.com/foto.jpg", 2148000));
            });
            return destinoId;
        }

        private static ClaimsPrincipal CrearPrincipal(Guid userId, string username)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(AbpClaimTypes.UserName, username),
                    new Claim(AbpClaimTypes.UserId, userId.ToString()),
                }));
        }

        [Fact]
        public async Task Should_Throw_When_Agregar_Without_Login()
        {
            var destinoId = await CrearDestinoAsync();

            using (_currentPrincipalAccessor.Change(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _seguimientoAppService.AgregarAsync(destinoId);
                });
            }
        }

        [Fact]
        public async Task Should_Agregar_Destino_A_La_Lista()
        {
            var destinoId = await CrearDestinoAsync();
            var userId = Guid.NewGuid();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId, "seguidor")))
            {
                await _seguimientoAppService.AgregarAsync(destinoId);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var seguimiento = await _seguimientoRepository.FirstOrDefaultAsync(
                    s => s.UserId == userId && s.DestinoId == destinoId);

                seguimiento.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task Should_Throw_When_Agregar_Destino_Inexistente()
        {
            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid(), "seguidor-inexistente")))
            {
                var exception = await Should.ThrowAsync<UserFriendlyException>(async () =>
                {
                    await _seguimientoAppService.AgregarAsync(Guid.NewGuid());
                });

                exception.Message.ShouldBe("El destino no existe.");
            }
        }

        [Fact]
        public async Task Should_Throw_When_Agregar_Duplicado()
        {
            var destinoId = await CrearDestinoAsync();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid(), "seguidor-duplicado")))
            {
                await _seguimientoAppService.AgregarAsync(destinoId);

                var exception = await Should.ThrowAsync<UserFriendlyException>(async () =>
                {
                    await _seguimientoAppService.AgregarAsync(destinoId);
                });

                exception.Message.ShouldBe("Este destino ya está en tu lista de seguimiento.");
            }
        }

        [Fact]
        public async Task Should_Eliminar_Destino_De_La_Lista()
        {
            var destinoId = await CrearDestinoAsync();
            var userId = Guid.NewGuid();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId, "seguidor-elimina")))
            {
                await _seguimientoAppService.AgregarAsync(destinoId);
                await _seguimientoAppService.EliminarAsync(destinoId);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var seguimiento = await _seguimientoRepository.FirstOrDefaultAsync(
                    s => s.UserId == userId && s.DestinoId == destinoId);

                seguimiento.ShouldBeNull();
            });
        }

        [Fact]
        public async Task Should_Throw_When_Eliminar_Destino_No_Seguido()
        {
            var destinoId = await CrearDestinoAsync();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid(), "seguidor-sin-lista")))
            {
                var exception = await Should.ThrowAsync<UserFriendlyException>(async () =>
                {
                    await _seguimientoAppService.EliminarAsync(destinoId);
                });

                exception.Message.ShouldBe("Este destino no está en tu lista de seguimiento.");
            }
        }

        [Fact]
        public async Task Should_Not_Eliminar_Seguimiento_De_Otro_Usuario()
        {
            var destinoId = await CrearDestinoAsync();
            var duenioId = Guid.NewGuid();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(duenioId, "duenio")))
            {
                await _seguimientoAppService.AgregarAsync(destinoId);
            }

            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid(), "otro-usuario")))
            {
                await Should.ThrowAsync<UserFriendlyException>(async () =>
                {
                    await _seguimientoAppService.EliminarAsync(destinoId);
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var seguimiento = await _seguimientoRepository.FirstOrDefaultAsync(
                    s => s.UserId == duenioId && s.DestinoId == destinoId);

                seguimiento.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task GetMiLista_Should_Return_Only_Own_Destinos()
        {
            var destinoParisId = await CrearDestinoAsync("Francia", "Paris");
            var destinoRomaId = await CrearDestinoAsync("Italia", "Roma");
            var destinoAjenoId = await CrearDestinoAsync("Japón", "Tokio");

            var userId = Guid.NewGuid();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId, "seguidor-lista")))
            {
                await _seguimientoAppService.AgregarAsync(destinoParisId);
                await _seguimientoAppService.AgregarAsync(destinoRomaId);
            }

            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid(), "otro-seguidor")))
            {
                await _seguimientoAppService.AgregarAsync(destinoAjenoId);
            }

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId, "seguidor-lista")))
            {
                var lista = await _seguimientoAppService.GetMiListaAsync();

                lista.Count.ShouldBe(2);
                lista.ShouldContain(x => x.Ciudad == "Paris" && x.Pais == "Francia");
                lista.ShouldContain(x => x.Ciudad == "Roma" && x.Pais == "Italia");
                lista.ShouldNotContain(x => x.Ciudad == "Tokio");
                lista.All(x => x.FechaAgregado != default).ShouldBeTrue();
                lista.All(x => x.UltimaActualizacion.HasValue).ShouldBeTrue();
            }
        }
    }
}
