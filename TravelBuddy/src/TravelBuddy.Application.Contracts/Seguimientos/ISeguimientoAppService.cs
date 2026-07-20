using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Seguimientos
{
    public interface ISeguimientoAppService : IApplicationService
    {
        Task AgregarAsync(Guid destinoId);

        Task EliminarAsync(Guid destinoId);

        Task<List<DestinoSeguidoDTO>> GetMiListaAsync();
    }
}
