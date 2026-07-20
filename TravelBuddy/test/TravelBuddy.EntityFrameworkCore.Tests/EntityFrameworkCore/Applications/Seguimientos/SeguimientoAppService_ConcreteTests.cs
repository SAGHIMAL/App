using TravelBuddy.Seguimientos;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Seguimientos
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class SeguimientoAppService_ConcreteTests : SeguimientoAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {

    }
}
