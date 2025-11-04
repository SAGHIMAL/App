using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Destinos;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities; 
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity; 
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using Xunit;

namespace TravelBuddy.Calificaciones
{
    public abstract class CalificacionAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly ICalificacionAppService _calificacionAppService;
        private readonly IRepository<Destino, Guid> _destinoRepository;
        private readonly IRepository<Calificacion, Guid> _calificacionRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
        private readonly IdentityUserManager _identityUserManager;
        // El constructor obtiene los servicios que necesitamos
        public CalificacionAppService_Tests()
        {
            _calificacionAppService = GetRequiredService<ICalificacionAppService>();
            _destinoRepository = GetRequiredService<IRepository<Destino, Guid>>();
            _calificacionRepository = GetRequiredService<IRepository<Calificacion, Guid>>();
            _currentUser = GetRequiredService<ICurrentUser>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
            _identityUserManager = GetRequiredService<IdentityUserManager>();
        }

        /*
         * PRUEBA DE SEGURIDAD (Punto 6.3 del TP)
         * "Asegurar que el endpoint falla con 401 si no se provee token."
         * [cite: 518]
         * En una prueba de integración, esto se traduce a una AbpAuthorizationException.
         */
        [Fact]
        public async Task NotCrearCalificacionWhenNotLogin()
        {
            // Arrange
            var destinoId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _destinoRepository.InsertAsync(new Destino(destinoId, "Francia", "Paris", "48.8566° N, 2.3522° E", "https://example.com/paris.jpg", 2148000));
            });

            var input = new crearCalificacionDTO

            {
                DestinoId = destinoId,
                Puntaje = 5
            };

            using (_currentPrincipalAccessor.Change(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                // --- ACT & ASSERT ---
                // Ahora que SÍ estamos deslogueados, la excepción de autorización SÍ se lanzará
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _calificacionAppService.CrearAsync(input);
                });
                // Act & Assert
                // Verificamos que al llamar al método (sin estar logueado),
                // se lanza la excepción de autorización que pusimos.
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
            {
                await _calificacionAppService.CrearAsync(input);
            });
            }
        }
        /*
         * PRUEBA DE INTEGRACIÓN (Punto 6.2 del TP)
         * "Confirmar que el AppService [...] requiere autenticación."
         * [cite: 517]
         * "Validar la lógica de calificación..."
         * 
         */

        [Fact]
        public async Task ShouldCreateCalificacionWhenLoggedIn()
        {
            // Arrange
            // 1. Creamos un destino de prueba en la BD en memoria
            var destinoId = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () => // <-- ¡AGREGA ESTO!
            {
                await _destinoRepository.InsertAsync(new Destino(destinoId, "Francia", "Paris", "48.8566° N, 2.3522° E", "https://example.com/paris.jpg", 2148000));
            });


            // 2. Simulamos un inicio de sesión
            var userId = Guid.NewGuid();
            var username = "testuser";

            await WithUnitOfWorkAsync(async () =>
            {
                var user = new IdentityUser(userId, username, "testuser@example.com");
                // Le damos una contraseña y guardamos el usuario
                var identityResult = await _identityUserManager.CreateAsync(user, "TestPassword123!");
                identityResult.Succeeded.ShouldBeTrue(); // ¡Verifica que el resultado fue exitoso!
            });
            var claimsprincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                        new Claim(AbpClaimTypes.UserName, username),
                        new Claim(AbpClaimTypes.UserId,userId.ToString()),
                        }));
                using (_currentPrincipalAccessor.Change(claimsprincipal))
                {
                    var input = new crearCalificacionDTO
                    {
                        DestinoId = destinoId,
                        Puntaje = 5,
                        Comentario = "Prueba de integración"
                    };
                    await _calificacionAppService.CrearAsync(input);
                    // Act
                    // Llamamos al método AHORA QUE ESTAMOS LOGUEADOS

                    await WithUnitOfWorkAsync(async () =>
                    {
                        var calificacion = await _calificacionRepository.FirstOrDefaultAsync(c => c.DestinoId == destinoId);

                        calificacion.ShouldNotBeNull();
                        calificacion.Puntaje.ShouldBe(5);
                        calificacion.Comentario.ShouldBe("Prueba de integración");
                        calificacion.UserId.ShouldBe(userId);
                    });
                }
            }
        }
    }

