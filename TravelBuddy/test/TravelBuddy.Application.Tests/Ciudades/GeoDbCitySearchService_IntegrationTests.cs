using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TravelBuddy.Ciudades
{
    public class GeoDbCitySearchService_IntegrationTests : TravelBuddyTestBase<TravelBuddyApplicationModule>
    {
        private readonly ICitySearchService _citySearchService;

        public GeoDbCitySearchService_IntegrationTests()
        {
            _citySearchService = ServiceProvider.GetRequiredService<ICitySearchService>();
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnRealCities_WhenApiIsCalled()
        {
            var nombreParcial = "Rio";
            var input = new SearchCityInputDTO { nombreParcial = nombreParcial };

            var result = await _citySearchService.SearchCitiesAsync(input);

          
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0); 

            var firstCity = result[0];
            firstCity.City.ShouldNotBeNullOrEmpty();
            firstCity.Country.ShouldNotBeNullOrEmpty();
            firstCity.Id.ShouldBeGreaterThan(0);
        }
    }
}
