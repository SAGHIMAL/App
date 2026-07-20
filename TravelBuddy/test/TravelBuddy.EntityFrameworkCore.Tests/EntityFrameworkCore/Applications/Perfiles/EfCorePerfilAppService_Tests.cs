using TravelBuddy.Perfiles;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Perfiles
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCorePerfilAppService_Tests : PerfilAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
