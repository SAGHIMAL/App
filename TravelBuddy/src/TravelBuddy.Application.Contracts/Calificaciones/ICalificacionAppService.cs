using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Calificaciones
{

    public interface ICalificacionAppService : IApplicationService
    {
        
        Task CrearAsync(crearCalificacionDTO input);

        
    }
}