using TravelBuddy.Notificaciones;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Notificaciones
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCoreNotificacionAppService_Tests : NotificacionAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
