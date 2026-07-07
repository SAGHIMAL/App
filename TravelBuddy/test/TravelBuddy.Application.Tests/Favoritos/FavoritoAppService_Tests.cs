using Shouldly;
using System;
using System.Threading.Tasks;
using TravelBuddy.Ciudades;
using TravelBuddy.Favoritos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace TravelBuddy.Favoritos
{
    public class FavoritoAppServiceTests : TravelBuddyApplicationTestBase<TravelBuddyApplicationTestModule>
    {
        private readonly IFavoritoAppService _favoritoAppService;
        private readonly IRepository<Ciudad, Guid> _ciudadRepository;
        private readonly IRepository<Favorito, Guid> _favoritoRepository;

        public FavoritoAppServiceTests()
        {
            _favoritoAppService = GetRequiredService<IFavoritoAppService>();
            _ciudadRepository = GetRequiredService<IRepository<Ciudad, Guid>>();
            _favoritoRepository = GetRequiredService<IRepository<Favorito, Guid>>();
        }

        [Fact]
        public async Task Should_Add_Destino_To_Favoritos()
        {
            // 1. Arrange
            var ciudadId = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () =>
            {
                await _ciudadRepository.InsertAsync(new Ciudad(
                    ciudadId, 101, "Cabo Polonio", "Uruguay", "Rocha", 50, -34.4, -53.7
                ));
            });

            // 2. Act: Pasamos SOLO el GUID (Corregido)
            await _favoritoAppService.CreateAsync(ciudadId);

            // 3. Assert
            await WithUnitOfWorkAsync(async () =>
            {
                var favorito = await _favoritoRepository.FirstOrDefaultAsync(f => f.DestinoId == ciudadId);
                favorito.ShouldNotBeNull();
                favorito.DestinoId.ShouldBe(ciudadId);
            });
        }

        [Fact]
        public async Task Should_Remove_Destino_From_Favoritos()
        {
            // 1. Arrange
            var ciudadId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                // Simulamos que ya existe en la DB
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), Guid.NewGuid(), ciudadId));
            });

            // 2. Act: Pasamos SOLO el GUID del destino (Corregido)
            await _favoritoAppService.DeleteAsync(ciudadId);

            // 3. Assert
            await WithUnitOfWorkAsync(async () =>
            {
                var favorito = await _favoritoRepository.FirstOrDefaultAsync(f => f.DestinoId == ciudadId);
                favorito.ShouldBeNull(); // Debe haberse borrado
            });
        }

        [Fact]
        public async Task Should_Get_My_List_Of_Favoritos()
        {
            // 1. Arrange
            var ciudadId1 = Guid.NewGuid();
            var ciudadId2 = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), Guid.NewGuid(), ciudadId1));
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), Guid.NewGuid(), ciudadId2));
            });

            // 2. Act: Llamamos SIN argumentos (Corregido)
            var result = await _favoritoAppService.GetListAsync();

            // 3. Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThanOrEqualTo(2);
            result.ShouldContain(x => x.DestinoId == ciudadId1);
        }
    }
}