using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Calificaciones
{

    public interface ICalificacionAppService : IApplicationService
    {
        // Define el método para crear una calificación,
        // recibe el DTO que acabas de crear.
        Task CrearAsync(crearCalificacionDTO input);

        // (En el futuro, aquí podrías agregar métodos como BorrarAsync, GetListAsync, etc.)
    }
}