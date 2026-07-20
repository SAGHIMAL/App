using Shouldly;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBuddy.Comentarios;
using TravelBuddy.Favoritos;
using TravelBuddy.Notificaciones;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace TravelBuddy.Comentarios
{
    public abstract class ComentarioAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IComentarioAppService _comentarioAppService;
        private readonly IRepository<Comentario, Guid> _comentarioRepository;
        private readonly IRepository<Favorito, Guid> _favoritoRepository;
        private readonly IRepository<Notificacion, Guid> _notificacionRepository;
        private readonly IRepository<PreferenciaNotificacion, Guid> _preferenciaRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public ComentarioAppService_Tests()
        {
            _comentarioAppService = GetRequiredService<IComentarioAppService>();
            _comentarioRepository = GetRequiredService<IRepository<Comentario, Guid>>();
            _favoritoRepository = GetRequiredService<IRepository<Favorito, Guid>>();
            _notificacionRepository = GetRequiredService<IRepository<Notificacion, Guid>>();
            _preferenciaRepository = GetRequiredService<IRepository<PreferenciaNotificacion, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        private static ClaimsPrincipal CrearPrincipal(Guid userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(AbpClaimTypes.UserName, "testuser-comentarios"),
                new Claim(AbpClaimTypes.UserId, userId.ToString())
            }));
        }

        [Fact]
        public async Task Should_Guardar_El_UserId_Del_Autor_Al_Comentar()
        {
            var userId = Guid.NewGuid();
            var destinoId = Guid.NewGuid();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                await _comentarioAppService.CreateAsync(new CrearComentarioDTO
                {
                    DestinoId = destinoId,
                    Texto = "Hermoso lugar, volvería sin dudarlo."
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var comentario = await _comentarioRepository.FirstOrDefaultAsync(c => c.DestinoId == destinoId);
                comentario.ShouldNotBeNull();
                comentario.UserId.ShouldBe(userId);
                comentario.Texto.ShouldBe("Hermoso lugar, volvería sin dudarlo.");
            });
        }

        [Fact]
        public async Task Should_Listar_Solo_Los_Comentarios_Del_Destino()
        {
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();
            var destino1 = Guid.NewGuid();
            var destino2 = Guid.NewGuid();

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userA)))
            {
                await _comentarioAppService.CreateAsync(new CrearComentarioDTO
                {
                    DestinoId = destino1,
                    Texto = "Comentario de A en destino 1"
                });
                await _comentarioAppService.CreateAsync(new CrearComentarioDTO
                {
                    DestinoId = destino2,
                    Texto = "Comentario de A en destino 2"
                });
            }

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userB)))
            {
                await _comentarioAppService.CreateAsync(new CrearComentarioDTO
                {
                    DestinoId = destino1,
                    Texto = "Comentario de B en destino 1"
                });
            }

            var result = await _comentarioAppService.GetListPorDestinoAsync(destino1);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            result.ShouldContain(c => c.Texto == "Comentario de A en destino 1" && c.UserId == userA);
            result.ShouldContain(c => c.Texto == "Comentario de B en destino 1" && c.UserId == userB);
            result.ShouldAllBe(c => c.DestinoId == destino1);
        }


        [Fact]
        public async Task Should_Notificar_A_Los_Fans_Del_Destino_Al_Comentar()
        {
            var autorId = Guid.NewGuid();
            var fanId = Guid.NewGuid();
            var destinoId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), destinoId, autorId));
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), destinoId, fanId));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(autorId)))
            {
                await _comentarioAppService.CreateAsync(new CrearComentarioDTO
                {
                    DestinoId = destinoId,
                    Texto = "Nuevo comentario para los fans"
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var notificaciones = await _notificacionRepository.GetListAsync(n => n.DestinoId == destinoId);
                notificaciones.Count.ShouldBe(1);
                notificaciones.Single().UserId.ShouldBe(fanId);
                notificaciones.Single().Leida.ShouldBeFalse();
            });
        }

        [Fact]
        public async Task Should_Not_Notificar_Al_Fan_Que_Desactivo_La_Preferencia()
        {
            var autorId = Guid.NewGuid();
            var fanConPreferenciaOff = Guid.NewGuid();
            var destinoId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), destinoId, fanConPreferenciaOff));

                var preferencia = new PreferenciaNotificacion(Guid.NewGuid(), fanConPreferenciaOff)
                {
                    NotificarComentariosEnFavoritos = false
                };
                await _preferenciaRepository.InsertAsync(preferencia);
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(autorId)))
            {
                await _comentarioAppService.CreateAsync(new CrearComentarioDTO
                {
                    DestinoId = destinoId,
                    Texto = "Comentario sin notificacion"
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var notificaciones = await _notificacionRepository.GetListAsync(n => n.DestinoId == destinoId);
                notificaciones.ShouldBeEmpty();
            });
        }
    }
}
