using System;
using TravelBuddy.Calificaciones;
using Xunit;

namespace TravelBuddy.EntityFrameworkCore.Applications.Calificaciones
{
    // 1. Esta clase NO es abstracta.
    // 2. Hereda de tu clase abstracta de pruebas.
    // 3. Le pasas tu módulo de pruebas real.
    public class CalificacionAppService_ConcreteTests : CalificacionAppService_Tests<TravelBuddyEntityFrameworkCoreTestModule>
    {
        // Esta clase se deja vacía.
        // El Explorador de Pruebas la encontrará y ejecutará
        // todas las pruebas [Fact] que heredó de la clase base.
    }
}