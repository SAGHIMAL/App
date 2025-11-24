using Microsoft.AspNetCore.Authorization; 
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.AspNetCore;
using Volo.Abp;



namespace TravelBuddy.Calificaciones
{
    [Authorize]

    public class CalificacionAppService : TravelBuddyAppService, ICalificacionAppService
    {

        private readonly IRepository<Calificacion, Guid> _calificacionRepository;


        private readonly ICurrentUser _currentUser;

        public CalificacionAppService(
            IRepository<Calificacion, Guid> calificacionRepository,
            ICurrentUser currentUser)
        {
            _calificacionRepository = calificacionRepository;
            _currentUser = currentUser;
        }

     
        public async Task CrearAsync(crearCalificacionDTO input)
        {
            
            if (!_currentUser.Id.HasValue)
            {
                
                throw new AbpAuthorizationException("No estás autorizado");
            }

            var existingRating = await _calificacionRepository.FirstOrDefaultAsync(
         c => c.UserId == _currentUser.Id && c.DestinoId == input.DestinoId
     );

            
            if (existingRating != null)
            {
                throw new UserFriendlyException("Ya has calificado este destino.");
            }

            var calificacion = new Calificacion(
                GuidGenerator.Create(), 
                input.DestinoId,        
                _currentUser.Id.Value, 
                input.Puntaje,
                input.Comentario
            );

           
            await _calificacionRepository.InsertAsync(calificacion);
        }
    }
}