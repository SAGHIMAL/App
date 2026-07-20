using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Perfiles
{
    public interface IPerfilAppService : IApplicationService
    {
        Task EliminarMiCuentaAsync();
        Task<PerfilPublicoDTO> GetPerfilPublicoAsync(Guid userId);
    }
}
