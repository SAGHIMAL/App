using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Ciudades
{
    public class CityAppService : ApplicationService, ICityAppService
    {
        private readonly ICitySearchService _citySearchService;

        public CityAppService(ICitySearchService citySearchService)
        {
            _citySearchService = citySearchService;
        }

        public async Task<List<CiudadDTO>> SearchCitiesAsync(SearchCityInputDTO input)
        {
            
            List<CiudadesExternasDTO> externalCities = await _citySearchService.SearchByNameAsync(input.nombreParcial);

            var cityDtos = externalCities.Select(extCity => new CiudadDTO
            {
                Id = extCity.Id,
                City = extCity.City,
                Country = extCity.Country,
                Region = extCity.Region
              
            }).ToList();

            return cityDtos;
        }
    }
}
