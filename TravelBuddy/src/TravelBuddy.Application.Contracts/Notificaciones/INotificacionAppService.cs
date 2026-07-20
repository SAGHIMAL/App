using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Notificaciones
{
    public interface INotificacionAppService : IApplicationService
    {
        Task CrearNotificacionPruebaAsync(string mensaje, Guid? destinoId);
        Task MarcarComoLeidaAsync(Guid id);
        Task MarcarComoNoLeidaAsync(Guid id);

        Task<List<NotificacionDTO>> GetNotificacionesASync();

        Task<PreferenciaNotificacionDTO> GetMisPreferenciasAsync();
        Task UpdateMisPreferenciasAsync(PreferenciaNotificacionDTO input);

    }


}
