using Shouldly;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBuddy.Ciudades;
using TravelBuddy.Favoritos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace TravelBuddy.Favoritos
{
    public abstract class FavoritoAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IFavoritoAppService _favoritoAppService;
        private readonly IRepository<Ciudad, Guid> _ciudadRepository;
        private readonly IRepository<Favorito, Guid> _favoritoRepository;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public FavoritoAppService_Tests()
        {
            _favoritoAppService = GetRequiredService<IFavoritoAppService>();
            _ciudadRepository = GetRequiredService<IRepository<Ciudad, Guid>>();
            _favoritoRepository = GetRequiredService<IRepository<Favorito, Guid>>();
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        private static ClaimsPrincipal CrearPrincipal(Guid userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(AbpClaimTypes.UserName, "testuser-favoritos"),
                new Claim(AbpClaimTypes.UserId, userId.ToString())
            }));
        }

        [Fact]
        public async Task Should_Add_Destino_To_Favoritos()
        {
            var userId = Guid.NewGuid();
            var ciudadId = Guid.NewGuid();
            await WithUnitOfWorkAsync(async () =>
            {
                await _ciudadRepository.InsertAsync(new Ciudad(
                    ciudadId, 101, "Cabo Polonio", "Uruguay", "Rocha", 50, -34.4, -53.7
                ));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                await _favoritoAppService.CreateAsync(ciudadId);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var favorito = await _favoritoRepository.FirstOrDefaultAsync(f => f.DestinoId == ciudadId);
                favorito.ShouldNotBeNull();
                favorito.DestinoId.ShouldBe(ciudadId);
                favorito.UserId.ShouldBe(userId);
            });
        }

        [Fact]
        public async Task Should_Remove_Destino_From_Favoritos()
        {
            var userId = Guid.NewGuid();
            var ciudadId = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), ciudadId, userId));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                await _favoritoAppService.DeleteAsync(ciudadId);
            }

            await WithUnitOfWorkAsync(async () =>
            {
                var favorito = await _favoritoRepository.FirstOrDefaultAsync(f => f.DestinoId == ciudadId);
                favorito.ShouldBeNull();
            });
        }

        [Fact]
        public async Task Should_Get_My_List_Of_Favoritos()
        {
            var userId = Guid.NewGuid();
            var ciudadId1 = Guid.NewGuid();
            var ciudadId2 = Guid.NewGuid();

            await WithUnitOfWorkAsync(async () =>
            {
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), ciudadId1, userId));
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), ciudadId2, userId));
                await _favoritoRepository.InsertAsync(new Favorito(Guid.NewGuid(), ciudadId1, Guid.NewGuid()));
            });

            using (_currentPrincipalAccessor.Change(CrearPrincipal(userId)))
            {
                var result = await _favoritoAppService.GetListAsync();

                result.ShouldNotBeNull();
                result.Count.ShouldBe(2);
                result.ShouldContain(x => x.DestinoId == ciudadId1);
                result.ShouldContain(x => x.DestinoId == ciudadId2);
            }
        }
    }
}
