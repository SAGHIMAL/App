
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Threading.Tasks;
using TravelBuddy.Ciudades;
using TravelBuddy.Experiencias;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Modularity; 
using Xunit;

namespace TravelBuddy.Experiencias
{
    public class ExperienciaAppServiceTests : TravelBuddyApplicationTestBase<TravelBuddyTestBaseModule>
    {
        private readonly IExperienciaAppService _experienciaAppService;
        private readonly IRepository<Ciudad, Guid> _ciudadRepository;
        private readonly IRepository<Experiencia, Guid> _experienciaRepository;

        public ExperienciaAppServiceTests()
        {
            _experienciaAppService = GetRequiredService<IExperienciaAppService>();
            _ciudadRepository = GetRequiredService<IRepository<Ciudad, Guid>>();
            _experienciaRepository = GetRequiredService<IRepository<Experiencia, Guid>>();
        }

        [Fact]
        public async Task Should_Create_A_Valid_Experiencia()
        {
            // 1. Arrange
            var ciudadId = Guid.NewGuid();

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

            // 2. Act
            var result = await _experienciaAppService.CreateAsync(input);

            // 3. Assert
            result.ShouldNotBeNull();
            result.Titulo.ShouldBe("Atardecer Mágico");
            result.Id.ShouldNotBe(Guid.Empty);

            // 4. Assert DB
            await WithUnitOfWorkAsync(async () =>
            {
                var experienciaEnDb = await _experienciaRepository.GetAsync(result.Id);
                experienciaEnDb.ShouldNotBeNull();
                experienciaEnDb.Titulo.ShouldBe("Atardecer Mágico");
                experienciaEnDb.CityID.ShouldBe(ciudadId);
            });
        }

        [Fact]
        public async Task Should_Get_List_Of_Experiencias()
        {
            // 1. Arrange
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

            // Insertamos datos iniciales
            await WithUnitOfWorkAsync(async () =>
            {
                await _experienciaRepository.InsertAsync(new Experiencia(
                    Guid.NewGuid(),
                    "Surf en La Pedrera",
                    "Olas gigantes",
                    ciudadId,
                    DateTime.Now,
                    TipoValoracion.Positiva
                ));
            });

            // 2. Act
            var result = await _experienciaAppService.GetListAsync(new GetExperienciasInput());

            // 3. Assert
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(x => x.Titulo == "Surf en La Pedrera");
        }
    }
}