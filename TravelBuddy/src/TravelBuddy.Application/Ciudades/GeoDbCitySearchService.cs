using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.DependencyInjection;
using TravelBuddy.Ciudades;

namespace TravelBuddy.Ciudades
{

    internal class GeoDbResponse
    {
        [JsonPropertyName("data")]
        public List<CiudadesExternasDTO> data { get; set; }
    }


    public class GeoDbCitySearchService : ICitySearchService, ITransientDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GeoDbCitySearchService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<CiudadesExternasDTO>> SearchByNameAsync(string nombreParcial)
        {
            var apiKey = _configuration["ExternalApis:GeoDb:ApiKey"];
            var apiHost = _configuration["ExternalApis:GeoDb:ApiHost"];
            var baseUrl = $"https://{apiHost}/v1/geo";

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", apiHost);

            string url = $"{baseUrl}/cities?namePrefix={Uri.EscapeDataString(nombreParcial)}&limit=5";


            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
               
                string jsonResult = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserializamos la respuesta completa
                var geoDbResponse = JsonSerializer.Deserialize<GeoDbResponse>(jsonResult, options);

                // Devolvemos solo la lista de ciudades (el "data")
                return geoDbResponse?.data ?? new List<CiudadesExternasDTO>();
            }
            else
            {
                return new List<CiudadesExternasDTO>();
            }
        }
    }
}
