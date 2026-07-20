using TravelBuddy.Administracion;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Administracion
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCoreMetricasApiExternaAppService_Tests : MetricasApiExternaAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
