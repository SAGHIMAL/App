using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Xunit;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using TravelBuddy.Destinos;

namespace TravelBuddy.Destinos;

public abstract class DestinoAppService_Tests<TStartupModule> : TravelBuddyApplicationTestBase<TStartupModule>
where TStartupModule : IAbpModule
{  
    private readonly IDestinoAppService _destinoAppService;
    protected DestinoAppService_Tests()
    {
        _destinoAppService = GetRequiredService<IDestinoAppService>();
    }
    [Fact]
    public async Task Should_Get_List_Of_Destinos()
    {

        //Este apartado es para crear un destino en vez de usar la seed
        //Deberiamos de dar de alta un destino desde el swagger pero da error
       /* await _destinoAppService.CreateAsync(new CreateUpdatedestinoDTO
        {
            Ciudad = "Paris",
            Coordenadas = "48.8566° N, 2.3522° E",
            Pais = "Francia",
            Foto = "https://example.com/paris.jpg",
            Poblacion = 2148000
        });
       */


        // Act
        var result = await _destinoAppService.GetListAsync(new PagedAndSortedResultRequestDto());
        // Assert
        //Console.WriteLine("resultado:",result);
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(x => x.Pais == "Francia");
    }


    [Fact]
    public async Task Should_Create_A_Destino()
    {
        // Act
        var result = await _destinoAppService.CreateAsync(
            new CreateUpdatedestinoDTO
            {
                Ciudad = "Paris",
                Coordenadas = "48.8566° N, 2.3522° E",
                Pais = "Francia",
                Foto = "https://example.com/paris.jpg",
                Poblacion = 2148000
            }


            );

        // Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Ciudad.ShouldBe("Paris");
    }
    [Fact]
    public async Task Should_Not_Create_A_Destino_Without_Pais()
    {
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinoAppService.CreateAsync(
                new CreateUpdatedestinoDTO
                {
                    Ciudad = "Paris",
                    Coordenadas = "48.8566° N, 2.3522° E",
                    Pais = "",
                    Foto = "https://example.com/paris.jpg",
                    Poblacion = 2148000
                }
            );
        });
        exception.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Pais"));
    }
}   




