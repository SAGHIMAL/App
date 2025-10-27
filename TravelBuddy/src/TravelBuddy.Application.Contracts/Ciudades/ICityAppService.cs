using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Ciudades
{
    public interface ICityAppService: IApplicationService
    {
        Task<List<CiudadDTO>> SearchCitiesAsync(SearchCityInputDTO input);
    }
}
