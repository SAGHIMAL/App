
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBuddy.Ciudades;
using TravelBuddy.Experiencias;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace TravelBuddy.Experiencias
{
    public abstract class ExperienciaAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IExperienciaAppService _experienciaAppService;
        private readonly IRepository<Ciudad, Guid> _ciudadRepository;
        private readonly IRepository<Experiencia, Guid> _experienciaRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public ExperienciaAppService_Tests()
        {
            _experienciaAppService = GetRequiredService<IExperienciaAppService>();
            _ciudadRepository = GetRequiredService<IRepository<Ciudad, Guid>>();
            _experienciaRepository = GetRequiredService<IRepository<Experiencia, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        private static ClaimsPrincipal CrearPrincipal(Guid userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(AbpClaimTypes.UserName, "testuser-experiencias"),
                new Claim(AbpClaimTypes.UserId, userId.ToString())
            }));
        }

        [Fact]
        public async Task Should_Create_A_Valid_Experiencia()
        {
            var ciudadId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                var ciudad = new Ciudad(
                    ciudadId,
                    123,
                    "Punta del Diablo",
                    "Uruguay",
                    "Rocha",
                    1000,
                    -34.8833,
                    -54.3333
                );
                await _ciudadRepository.InsertAsync(ciudad);
            });

            var input = new CrearExperienciaDTO
            {
                Titulo = "Atardecer Mágico",
                Descripcion = "Fue una experiencia inolvidable con amigos.",
                FechaExperiencia = DateTime.Now,
                Valoracion = TipoValoracion.Positiva,
                CityID = ciudadId
            };

            ExperienciaDTO result;
            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                result = await _experienciaAppService.CreateAsync(input);
            }

            result.ShouldNotBeNull();
            result.Titulo.ShouldBe("Atardecer Mágico");
            result.Id.ShouldNotBe(Guid.Empty);

            await WithUnitOfWorkAsync(async () =>
            {
                var experienciaEnDb = await _experienciaRepository.GetAsync(result.Id);
                experienciaEnDb.ShouldNotBeNull();
                experienciaEnDb.Titulo.ShouldBe("Atardecer Mágico");
                experienciaEnDb.CityID.ShouldBe(ciudadId);
                experienciaEnDb.UserId.ShouldBe(userId);
            });
        }

        [Fact]
        public async Task Should_Get_List_Of_Experiencias()
        {
            var ciudadId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _ciudadRepository.InsertAsync(new Ciudad(
                    ciudadId,
                    456,
                    "La Pedrera",
                    "Uruguay",
                    "Rocha",
                    500,
                    -34.0000,
                    -54.0000
                ));
            });

            await WithUnitOfWorkAsync(async () =>
            {
                await _experienciaRepository.InsertAsync(new Experiencia(
                    Guid.NewGuid(),
                    "Surf en La Pedrera",
                    "Olas gigantes",
                    ciudadId,
                    Guid.NewGuid(),
                    DateTime.Now,
                    TipoValoracion.Positiva
                ));
            });

            var result = await _experienciaAppService.GetListAsync(new GetExperienciasInput());

            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(x => x.Titulo == "Surf en La Pedrera");
        }


        [Fact]
        public async Task Should_Update_Own_Experiencia()
        {
            var userId = Guid.NewGuid();
            var experienciaId = Guid.NewGuid();
            var ciudadId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _experienciaRepository.InsertAsync(new Experiencia(
                    experienciaId, "Titulo original", "Descripcion original",
                    ciudadId, userId, DateTime.Now, TipoValoracion.Neutral));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                var result = await _experienciaAppService.UpdateAsync(experienciaId, new CrearExperienciaDTO
                {
                    CityID = Guid.NewGuid(),
                    Titulo = "Titulo editado",
                    Descripcion = "Descripcion editada",
                    FechaExperiencia = DateTime.Now,
                    Valoracion = TipoValoracion.Positiva
                });

                result.Titulo.ShouldBe("Titulo editado");
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var experiencia = await _experienciaRepository.GetAsync(experienciaId);
                experiencia.Titulo.ShouldBe("Titulo editado");
                experiencia.Descripcion.ShouldBe("Descripcion editada");
                experiencia.Valoracion.ShouldBe(TipoValoracion.Positiva);
                experiencia.CityID.ShouldBe(ciudadId);
                experiencia.UserId.ShouldBe(userId);
            });
        }

        [Fact]
        public async Task Should_Not_Update_Experiencia_De_Otro_Usuario()
        {
            var duenioId = Guid.NewGuid();
            var atacanteId = Guid.NewGuid();
            var experienciaId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _experienciaRepository.InsertAsync(new Experiencia(
                    experienciaId, "Del duenio", "Descripcion del duenio",
                    Guid.NewGuid(), duenioId, DateTime.Now, TipoValoracion.Positiva));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(atacanteId)))
            {
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _experienciaAppService.UpdateAsync(experienciaId, new CrearExperienciaDTO
                    {
                        CityID = Guid.NewGuid(),
                        Titulo = "Hackeado",
                        Descripcion = "Hackeado",
                        FechaExperiencia = DateTime.Now,
                        Valoracion = TipoValoracion.Negativa
                    });
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var experiencia = await _experienciaRepository.GetAsync(experienciaId);
                experiencia.Titulo.ShouldBe("Del duenio");
            });
        }

        [Fact]
        public async Task Should_Delete_Own_Experiencia()
        {
            var userId = Guid.NewGuid();
            var experienciaId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _experienciaRepository.InsertAsync(new Experiencia(
                    experienciaId, "Para borrar", "Descripcion",
                    Guid.NewGuid(), userId, DateTime.Now, TipoValoracion.Negativa));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                await _experienciaAppService.DeleteAsync(experienciaId);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var experiencia = await _experienciaRepository.FirstOrDefaultAsync(e => e.Id == experienciaId);
                experiencia.ShouldBeNull();
            });
        }

        [Fact]
        public async Task Should_Not_Delete_Experiencia_De_Otro_Usuario()
        {
            var duenioId = Guid.NewGuid();
            var atacanteId = Guid.NewGuid();
            var experienciaId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _experienciaRepository.InsertAsync(new Experiencia(
                    experienciaId, "Del duenio", "Descripcion del duenio",
                    Guid.NewGuid(), duenioId, DateTime.Now, TipoValoracion.Positiva));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(atacanteId)))
            {
                await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                {
                    await _experienciaAppService.DeleteAsync(experienciaId);
                });
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var experiencia = await _experienciaRepository.FirstOrDefaultAsync(e => e.Id == experienciaId);
                experiencia.ShouldNotBeNull();
            });
        }
    }
}
