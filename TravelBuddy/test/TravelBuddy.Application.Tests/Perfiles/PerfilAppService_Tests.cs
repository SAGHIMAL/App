using Shouldly;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBuddy.Experiencias;
using TravelBuddy.Perfiles;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace TravelBuddy.Perfiles
{
    public abstract class PerfilAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IPerfilAppService _perfilAppService;
        private readonly IProfileAppService _profileAppService;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IRepository<Experiencia, Guid> _experienciaRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public PerfilAppService_Tests()
        {
            _perfilAppService = GetRequiredService<IPerfilAppService>();
            _profileAppService = GetRequiredService<IProfileAppService>();
            _identityUserManager = GetRequiredService<IdentityUserManager>();
            _experienciaRepository = GetRequiredService<IRepository<Experiencia, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        private static ClaimsPrincipal CrearPrincipal(Guid userId, string username)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(AbpClaimTypes.UserName, username),
                new Claim(AbpClaimTypes.UserId, userId.ToString())
            }));
        }

        private async Task<Guid> CrearUsuarioAsync(string username, Action<IdentityUser>? configurar = null)
        {
            var userId = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () =>
            {
                var user = new IdentityUser(userId, username, $"{username}@example.com");
                configurar?.Invoke(user);
                (await _identityUserManager.CreateAsync(user, "TestPassword123!")).Succeeded.ShouldBeTrue();
            });
            return userId;
        }


        [Fact]
        public async Task Should_Eliminar_Mi_Propia_Cuenta()
        {
            var userId = await CrearUsuarioAsync("testuser-borrarme");

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId, "testuser-borrarme")))
            {
                await _perfilAppService.EliminarMiCuentaAsync();
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var user = await _identityUserManager.FindByIdAsync(userId.ToString());
                user.ShouldBeNull();
            });
        }

        [Fact]
        public async Task Should_Not_Eliminar_Cuenta_Sin_Estar_Logueado()
        {
            using (_currentPrincipalAccessor.Change(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _perfilAppService.EliminarMiCuentaAsync();
                });
            }
        }


        [Fact]
        public async Task Should_Devolver_Perfil_Publico_Con_Experiencias()
        {
            var userId = await CrearUsuarioAsync("testuser-publico", user =>
            {
                user.Name = "Ana";
                user.Surname = "Gomez";
                user.SetProperty("Foto", "https://example.com/ana.jpg");
            });
            var otroUserId = await CrearUsuarioAsync("testuser-otro");

            var cityId = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () =>
            {
                await _experienciaRepository.InsertAsync(new Experiencia(
                    Guid.NewGuid(), "Viaje a Paris", "Muy lindo", cityId, userId,
                    DateTime.Today.AddDays(-10), TipoValoracion.Positiva));
                await _experienciaRepository.InsertAsync(new Experiencia(
                    Guid.NewGuid(), "Viaje a Roma", "Regular", cityId, userId,
                    DateTime.Today.AddDays(-5), TipoValoracion.Neutral));
                await _experienciaRepository.InsertAsync(new Experiencia(
                    Guid.NewGuid(), "Viaje ajeno", "De otro", cityId, otroUserId,
                    DateTime.Today, TipoValoracion.Negativa));
            });

            using (_currentPrincipalAccessor.Change(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                var perfil = await _perfilAppService.GetPerfilPublicoAsync(userId);

                perfil.UserId.ShouldBe(userId);
                perfil.UserName.ShouldBe("testuser-publico");
                perfil.Nombre.ShouldBe("Ana");
                perfil.Apellido.ShouldBe("Gomez");
                perfil.Foto.ShouldBe("https://example.com/ana.jpg");
                perfil.Experiencias.Count.ShouldBe(2);
                perfil.Experiencias.ShouldAllBe(e => e.UserId == userId);
                perfil.Experiencias[0].Titulo.ShouldBe("Viaje a Roma");
            }
        }

        [Fact]
        public async Task Should_Throw_Cuando_El_Usuario_No_Existe()
        {
            await Should.ThrowAsync<UserFriendlyException>(async () =>
            {
                await _perfilAppService.GetPerfilPublicoAsync(Guid.NewGuid());
            });
        }


        [Fact]
        public async Task Should_Actualizar_Foto_Via_Profile_Api()
        {
            var userId = await CrearUsuarioAsync("testuser-foto");

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId, "testuser-foto")))
            {
                var input = new UpdateProfileDto
                {
                    UserName = "testuser-foto",
                    Email = "testuser-foto@example.com"
                };
                input.SetProperty("Foto", "https://example.com/nueva-foto.jpg");

                await _profileAppService.UpdateAsync(input);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var user = await _identityUserManager.GetByIdAsync(userId);
                user.GetProperty<string?>("Foto").ShouldBe("https://example.com/nueva-foto.jpg");
            });
        }
    }
}
