using TravelBuddy.Favoritos;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Favoritos
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCoreFavoritoAppService_Tests : FavoritoAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
