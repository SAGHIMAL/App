using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TravelBuddy.Ciudades
{
    public class CityAppService : ApplicationService, ICityAppService
    {
        private readonly ICitySearchService _citySearchService;
        private readonly IRepository<Ciudad, Guid> _cityRepository;

        public CityAppService(ICitySearchService citySearchService, IRepository<Ciudad, Guid> ciudadRepository)
        {
            _citySearchService = citySearchService;
            _cityRepository = ciudadRepository;
        }

        public async Task<List<CiudadDTO>> SearchCitiesAsync(SearchCityInputDTO input)
        {
            
            List<CiudadesExternasDTO> externalCities = await _citySearchService.SearchCitiesAsync(input);

            var cityDtos = externalCities.Select(extCity => new CiudadDTO
            {
                Id = extCity.Id,
                City = extCity.City,
                Country = extCity.Country,
                Region = extCity.Region
              
            }).ToList();

            return cityDtos;
        }

        public async Task<CiudadDTO> GetCityDetailAsync(int geoDBId)
        {

            var ciudadLocal = await _cityRepository.FirstOrDefaultAsync(x => x.GeoDbId == geoDBId);

            if (ciudadLocal != null)
            {
                return new CiudadDTO
                {
                    Id = ciudadLocal.GeoDbId,
                    City = ciudadLocal.Nombre,
                    Country = ciudadLocal.Pais,
                    Region = ciudadLocal.Region,
                    Population = ciudadLocal.Poblacion,
                    Latitud = ciudadLocal.Latitud,
                    Longitud = ciudadLocal.Longitud

                };
            }

            var ciudadExterna = await _citySearchService.GetCityByIdAsync(geoDBId);

            if (ciudadExterna == null)
            {
                throw new UserFriendlyException("La ciudad no fue encontrada.");
            }

            var nuevaCiudad = new Ciudad(
                GuidGenerator.Create(),
                ciudadExterna.Id,
                ciudadExterna.City,
                ciudadExterna.Country,
                ciudadExterna.Region,
                ciudadExterna.Population ?? 0,
                ciudadExterna.Latitude ?? 0,
                ciudadExterna.Longitude ?? 0
            );

            //nuevaCiudad.Latitud = ciudadExterna.Latitude ?? 0;
            //nuevaCiudad.Longitud = ciudadExterna.Longitude ?? 0;
            await _cityRepository.InsertAsync(nuevaCiudad);

            return new CiudadDTO
            {
                Id = nuevaCiudad.GeoDbId,
                City = nuevaCiudad.Nombre,
                Country = nuevaCiudad.Pais,
                Region = nuevaCiudad.Region,
                Population = nuevaCiudad.Poblacion,
                Latitud = nuevaCiudad.Latitud,
                Longitud = nuevaCiudad.Longitud
            };
        }



    }
}
