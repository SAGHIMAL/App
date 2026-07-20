using TravelBuddy.Ciudades;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Ciudades
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCoreCiudadesPopulares_Tests : CiudadesPopulares_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
