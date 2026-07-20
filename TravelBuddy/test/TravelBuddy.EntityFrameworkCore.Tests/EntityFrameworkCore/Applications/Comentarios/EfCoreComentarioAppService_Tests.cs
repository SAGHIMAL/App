using TravelBuddy.Comentarios;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Comentarios
{
    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class EfCoreComentarioAppService_Tests : ComentarioAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
    }
}
