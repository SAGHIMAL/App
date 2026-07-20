using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelBuddy.ApisExternas;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace TravelBuddy.Ciudades
{

    internal class GeoDbResponse
    {
        [JsonPropertyName("data")]
        public List<CiudadesExternasDTO> data { get; set; }
    }

    internal class GeoDbSingleResponse
    {
        [JsonPropertyName("data")]
        public CiudadesExternasDTO data { get; set; }
    }


    public class GeoDbCitySearchService : ICitySearchService, ITransientDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public GeoDbCitySearchService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _guidGenerator = guidGenerator;
            _unitOfWorkManager = unitOfWorkManager;
        }

        private async Task RegistrarLlamadaAsync(string endpoint, bool exitosa)
        {
            var repositorio = _serviceProvider.GetService<IRepository<RegistroLlamadaApi, Guid>>();
            if (repositorio == null)
            {
                return;
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                await repositorio.InsertAsync(
                    new RegistroLlamadaApi(_guidGenerator.Create(), "GeoDb", endpoint, exitosa));
                await uow.CompleteAsync();
            }
        }

        public async Task<List<CiudadesExternasDTO>> SearchCitiesAsync(SearchCityInputDTO input)
        {
            var apiKey = _configuration["ExternalApis:GeoDb:ApiKey"];
            var apiHost = _configuration["ExternalApis:GeoDb:ApiHost"];
            var baseUrl = $"https://{apiHost}/v1/geo";

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", apiHost);

            var urlBuilder = new System.Text.StringBuilder($"{baseUrl}/cities?limit=5");

            if (!string.IsNullOrEmpty(input.nombreParcial))
            {
                urlBuilder.Append($"&namePrefix={Uri.EscapeDataString(input.nombreParcial)}");
            }

            if (!string.IsNullOrEmpty(input.paisId))
            {
                urlBuilder.Append($"&countryIds={Uri.EscapeDataString(input.paisId)}");
            }



            if (input.minPoblacion.HasValue)
            {
                urlBuilder.Append($"&minPopulation={input.minPoblacion.Value}");
            }

            string url = urlBuilder.ToString();

            HttpResponseMessage response = await client.GetAsync(url);

            await RegistrarLlamadaAsync("cities (busqueda)", response.IsSuccessStatusCode);

            if (response.IsSuccessStatusCode)
            {
               
                string jsonResult = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var geoDbResponse = JsonSerializer.Deserialize<GeoDbResponse>(jsonResult, options);

                return geoDbResponse?.data ?? new List<CiudadesExternasDTO>();
            }
            else
            {
                return new List<CiudadesExternasDTO>();
            }
        }

        public async Task<CiudadesExternasDTO> GetCityByIdAsync(int geoDbId)
        {
            var apiKey = _configuration["ExternalApis:GeoDb:ApiKey"];
            var apiHost = _configuration["ExternalApis:GeoDb:ApiHost"];
            var baseUrl = $"https://{apiHost}/v1/geo";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", apiHost);

            string url = $"{baseUrl}/cities/{Uri.EscapeDataString(geoDbId.ToString())}";

            HttpResponseMessage response = await client.GetAsync(url);

            await RegistrarLlamadaAsync("cities/{id} (detalle)", response.IsSuccessStatusCode);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var ciudad = JsonSerializer.Deserialize<GeoDbSingleResponse>(jsonResult, options);
                return ciudad?.data;
            }
           
            return null;
        }

    }
}
