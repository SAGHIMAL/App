using Shouldly;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBuddy.Notificaciones;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace TravelBuddy.Notificaciones
{
    public abstract class NotificacionAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly INotificacionAppService _notificacionAppService;
        private readonly IRepository<Notificacion, Guid> _notificacionRepository;
        private readonly IRepository<PreferenciaNotificacion, Guid> _preferenciaRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public NotificacionAppService_Tests()
        {
            _notificacionAppService = GetRequiredService<INotificacionAppService>();
            _notificacionRepository = GetRequiredService<IRepository<Notificacion, Guid>>();
            _preferenciaRepository = GetRequiredService<IRepository<PreferenciaNotificacion, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        private static ClaimsPrincipal CrearPrincipal(Guid userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(AbpClaimTypes.UserName, "testuser-notificaciones"),
                new Claim(AbpClaimTypes.UserId, userId.ToString())
            }));
        }

        [Fact]
        public async Task Should_Listar_Solo_Mis_Notificaciones()
        {
            var miUserId = Guid.NewGuid();
            var otroUserId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _notificacionRepository.InsertAsync(new Notificacion(Guid.NewGuid(), miUserId, "Mia 1"));
                await _notificacionRepository.InsertAsync(new Notificacion(Guid.NewGuid(), miUserId, "Mia 2"));
                await _notificacionRepository.InsertAsync(new Notificacion(Guid.NewGuid(), otroUserId, "Ajena"));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(miUserId)))
            {
                var result = await _notificacionAppService.GetNotificacionesASync();

                result.Count.ShouldBe(2);
                result.ShouldContain(n => n.Mensaje == "Mia 1");
                result.ShouldContain(n => n.Mensaje == "Mia 2");
                result.ShouldNotContain(n => n.Mensaje == "Ajena");
            }
        }


        [Fact]
        public async Task Should_Marcar_Como_Leida_Y_Como_No_Leida()
        {
            var userId = Guid.NewGuid();
            var notificacionId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _notificacionRepository.InsertAsync(new Notificacion(notificacionId, userId, "Para leer"));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                await _notificacionAppService.MarcarComoLeidaAsync(notificacionId);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                (await _notificacionRepository.GetAsync(notificacionId)).Leida.ShouldBeTrue();
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                await _notificacionAppService.MarcarComoNoLeidaAsync(notificacionId);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                (await _notificacionRepository.GetAsync(notificacionId)).Leida.ShouldBeFalse();
            });
        }

        [Fact]
        public async Task Should_Not_Marcar_Notificacion_De_Otro_Usuario()
        {
            var duenioId = Guid.NewGuid();
            var atacanteId = Guid.NewGuid();
            var notificacionId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _notificacionRepository.InsertAsync(new Notificacion(notificacionId, duenioId, "Del duenio"));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(atacanteId)))
            {
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _notificacionAppService.MarcarComoLeidaAsync(notificacionId);
                });

                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _notificacionAppService.MarcarComoNoLeidaAsync(notificacionId);
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                (await _notificacionRepository.GetAsync(notificacionId)).Leida.ShouldBeFalse();
            });
        }


        [Fact]
        public async Task Should_Devolver_Preferencias_Default_Si_No_Configuro_Nada()
        {
            using (_currentPrincipalAccessor.Change(CrearPrincipal(Guid.NewGuid())))
            {
                var prefs = await _notificacionAppService.GetMisPreferenciasAsync();

                prefs.NotificarComentariosEnFavoritos.ShouldBeTrue();
                prefs.NotificarEventosDestinos.ShouldBeTrue();
            }
        }

        [Fact]
        public async Task Should_Crear_Y_Actualizar_Mis_Preferencias()
        {
            var userId = Guid.NewGuid();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                await _notificacionAppService.UpdateMisPreferenciasAsync(new PreferenciaNotificacionDTO
                {
                    NotificarComentariosEnFavoritos = false,
                    NotificarEventosDestinos = true
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var pref = await _preferenciaRepository.FirstOrDefaultAsync(p => p.UserId == userId);
                pref.ShouldNotBeNull();
                pref.NotificarComentariosEnFavoritos.ShouldBeFalse();
                pref.NotificarEventosDestinos.ShouldBeTrue();
            });
        }
    }
}
