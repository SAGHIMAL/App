using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using TravelBuddy.Favoritos;

namespace TravelBuddy.Favoritos
{
    [Authorize]
    public class FavoritoAppService : ApplicationService, IFavoritoAppService
    {
       
        private readonly IRepository<Favorito, Guid> _favoritoRepository; 

        public FavoritoAppService(IRepository<Favorito, Guid> favoritoRepository)
        {
            _favoritoRepository = favoritoRepository;
        }

        
        public async Task CreateAsync(Guid destinoId)
        {
            var userId = CurrentUser.GetId();

            var query = await _favoritoRepository.GetQueryableAsync();

            bool existe = query.Any(x => x.UserId == userId && x.DestinoId == destinoId);

            if (existe)
            {
                throw new UserFriendlyException("Este destino ya está en tus favoritos.");
            }

            var favorito = new Favorito(GuidGenerator.Create(), destinoId, userId);
            await _favoritoRepository.InsertAsync(favorito);
        }

        public async Task DeleteAsync(Guid destinoId)
        {
            var userId = CurrentUser.GetId();
            var query = await _favoritoRepository.GetQueryableAsync();

            var favorito = query.FirstOrDefault(x => x.UserId == userId && x.DestinoId == destinoId);

            if (favorito != null)
            {
                await _favoritoRepository.DeleteAsync(favorito);
            }
        }

        public async Task<List<FavoritoDTO>> GetListAsync()
        {
            var userId = CurrentUser.GetId();
            var list = await _favoritoRepository.GetListAsync(x => x.UserId == userId);
            return ObjectMapper.Map<List<Favorito>, List<FavoritoDTO>>(list);
        }
    }
}