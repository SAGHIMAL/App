using NSubstitute; 
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TravelBuddy.Ciudades
{
    public class CityAppService_Tests : TravelBuddyApplicationTestBase<TravelBuddyApplicationModule>
    {
        private readonly ICitySearchService _citySearchServiceMock;
        private readonly CityAppService _cityAppService;

        public CityAppService_Tests()
        {
            _citySearchServiceMock = Substitute.For<ICitySearchService>();

            _cityAppService = new CityAppService(_citySearchServiceMock);
        }


        [Fact]
        public async Task SearchCitiesReturnMappedCities()
        {
           
            var nombreParcial = "Santiag";
            var inputDto = new SearchCityInputDTO { nombreParcial = nombreParcial };

           
            var fakeApiResult = new List<CiudadesExternasDTO>
            {
                new CiudadesExternasDTO { Id = 1, City = "Santiago", Country = "Chile", Region = "Metropolitana" },
                new CiudadesExternasDTO { Id = 2, City = "Santiago", Country = "España", Region = "Galicia" }
            };

            _citySearchServiceMock
                .SearchByNameAsync(nombreParcial)
                .Returns(Task.FromResult(fakeApiResult));

            var result = await _cityAppService.SearchCitiesAsync(inputDto);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            result[0].City.ShouldBe("Santiago");
            result[0].Country.ShouldBe("Chile");
            result[1].Id.ShouldBe(2);
        }


        [Fact]
        public async Task SearchCitiesReturnEmptyList()
        { 
            var nombreParcial = "CiudadQueNoExiste";
            var inputDto = new SearchCityInputDTO { nombreParcial = nombreParcial };

            _citySearchServiceMock
                .SearchByNameAsync(nombreParcial)
                .Returns(Task.FromResult(new List<CiudadesExternasDTO>()));

            var result = await _cityAppService.SearchCitiesAsync(inputDto);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(0);
        }



    }
}
