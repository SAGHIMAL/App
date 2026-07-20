using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Calificaciones;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TravelBuddy.Ciudades
{
    public class CityAppService : ApplicationService, ICityAppService
    {
        private readonly ICitySearchService _citySearchService;
        private readonly IRepository<Ciudad, Guid> _cityRepository;
        private readonly IRepository<Calificacion, Guid> _calificacionRepository;

        public CityAppService(
            ICitySearchService citySearchService,
            IRepository<Ciudad, Guid> ciudadRepository,
            IRepository<Calificacion, Guid> calificacionRepository)
        {
            _citySearchService = citySearchService;
            _cityRepository = ciudadRepository;
            _calificacionRepository = calificacionRepository;
        }

        public async Task<List<CiudadPopularDTO>> GetCiudadesPopularesAsync(int cantidad = 10)
        {
            cantidad = Math.Clamp(cantidad, 1, 50);

            var calificaciones = await _calificacionRepository.GetListAsync();
            var ciudades = await _cityRepository.GetListAsync();

            return calificaciones
                .GroupBy(c => c.DestinoId)
                .Join(ciudades,
                    grupo => grupo.Key,
                    ciudad => ciudad.Id,
                    (grupo, ciudad) => new CiudadPopularDTO
                    {
                        GeoDbId = ciudad.GeoDbId,
                        IdLocal = ciudad.Id,
                        Nombre = ciudad.Nombre,
                        Pais = ciudad.Pais,
                        Region = ciudad.Region,
                        Promedio = grupo.Average(c => c.Puntaje),
                        CantidadCalificaciones = grupo.Count()
                    })
                .OrderByDescending(p => p.CantidadCalificaciones)
                .ThenByDescending(p => p.Promedio)
                .Take(cantidad)
                .ToList();
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
                    IdLocal = ciudadLocal.Id,
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

            await _cityRepository.InsertAsync(nuevaCiudad);

            return new CiudadDTO
            {
                Id = nuevaCiudad.GeoDbId,
                IdLocal = nuevaCiudad.Id,
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
