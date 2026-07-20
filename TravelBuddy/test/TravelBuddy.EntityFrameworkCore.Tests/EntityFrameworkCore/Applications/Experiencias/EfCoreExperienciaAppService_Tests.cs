using TravelBuddy.Experiencias;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Experiencias
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCoreExperienciaAppService_Tests : ExperienciaAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
