using System;
using TravelBuddy.Calificaciones;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Calificaciones
{

    [Collection(TravelBuddyTestConsts.CollectionDefinitionName)]
    public class CalificacionAppService_ConcreteTests : CalificacionAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
       
    }
}