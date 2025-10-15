using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TravelBuddy.Destinos;

namespace TravelBuddy.EntityFrameworkCore.Applications.Destinos;

[CollectionDefinition(TravelBuddyTestConsts.CollectionDefinitionName)]
public class EfCoreDestinoAppService_Test: DestinoAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
{

}
