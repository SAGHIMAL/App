using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Favoritos
{
    public interface IFavoritoAppService : IApplicationService
    {
        Task CreateAsync(Guid destinoId);  // este es 6.1
        Task DeleteAsync(Guid destinoId); //6.2

        Task<List<FavoritoDTO>> GetListAsync();    //6.3 
    }
}
