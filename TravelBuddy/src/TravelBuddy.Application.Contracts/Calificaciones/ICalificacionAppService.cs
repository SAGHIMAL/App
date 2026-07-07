using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;


namespace TravelBuddy.Calificaciones
{

    public interface ICalificacionAppService : 
        ICrudAppService<
            CalificacionDTO,
            Guid,
            GetCalificacionesInput,
            crearCalificacionDTO>
    {
        Task<double> GetPromedioAsync(Guid destinoId);
    }
}

