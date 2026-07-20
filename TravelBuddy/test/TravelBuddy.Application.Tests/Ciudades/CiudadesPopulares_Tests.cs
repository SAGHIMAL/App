using Shouldly;
using System;
using System.Threading.Tasks;
using TravelBuddy.Calificaciones;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace TravelBuddy.Ciudades
{
    public abstract class CiudadesPopulares_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly ICityAppService _cityAppService;
        private readonly IRepository<Ciudad, Guid> _ciudadRepository;
        private readonly IRepository<Calificacion, Guid> _calificacionRepository;

        public CiudadesPopulares_Tests()
        {
            _cityAppService = GetRequiredService<ICityAppService>();
            _ciudadRepository = GetRequiredService<IRepository<Ciudad, Guid>>();
            _calificacionRepository = GetRequiredService<IRepository<Calificacion, Guid>>();
        }

        private async Task<Guid> CrearCiudadAsync(int geoDbId, string nombre)
        {
            var id = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () =>
            {
                await _ciudadRepository.InsertAsync(
                    new Ciudad(id, geoDbId, nombre, "Argentina", "Region", 1000, -32.48, -58.23));
            });
            return id;
        }

        private async Task CalificarAsync(Guid ciudadId, params int[] puntajes)
        {
            await WithUnitOfWorkAsync(async () =>
            {
                foreach (var puntaje in puntajes)
                {
                    await _calificacionRepository.InsertAsync(
                        new Calificacion(Guid.NewGuid(), ciudadId, Guid.NewGuid(), puntaje, null));
                }
            });
        }

        [Fact]
        public async Task Should_Listar_Populares_Ordenadas_Por_Cantidad_Y_Promedio()
        {
            var masCalificadaId = await CrearCiudadAsync(101, "MasCalificada");
            var mejorPuntuadaId = await CrearCiudadAsync(102, "MejorPuntuada");
            await CrearCiudadAsync(103, "SinCalificar");

            await CalificarAsync(masCalificadaId, 3, 3, 3);
            await CalificarAsync(mejorPuntuadaId, 5, 5);

            await CalificarAsync(Guid.NewGuid(), 5);

            var populares = await _cityAppService.GetCiudadesPopularesAsync();

            populares.Count.ShouldBe(2);
            populares[0].Nombre.ShouldBe("MasCalificada");
            populares[0].CantidadCalificaciones.ShouldBe(3);
            populares[0].Promedio.ShouldBe(3);
            populares[1].Nombre.ShouldBe("MejorPuntuada");
            populares[1].CantidadCalificaciones.ShouldBe(2);
            populares[1].Promedio.ShouldBe(5);
            populares[0].GeoDbId.ShouldBe(101);
        }

        [Fact]
        public async Task Should_Respetar_La_Cantidad_Pedida()
        {
            var ciudad1 = await CrearCiudadAsync(201, "Popular1");
            var ciudad2 = await CrearCiudadAsync(202, "Popular2");

            await CalificarAsync(ciudad1, 4, 4);
            await CalificarAsync(ciudad2, 5);

            var populares = await _cityAppService.GetCiudadesPopularesAsync(1);

            populares.Count.ShouldBe(1);
            populares[0].Nombre.ShouldBe("Popular1");
        }

        [Fact]
        public async Task Should_Devolver_Lista_Vacia_Sin_Calificaciones()
        {
            var populares = await _cityAppService.GetCiudadesPopularesAsync();
            populares.ShouldBeEmpty();
        }
    }
}
