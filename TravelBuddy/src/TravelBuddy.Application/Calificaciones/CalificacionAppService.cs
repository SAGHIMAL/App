using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using TravelBuddy.Calificaciones;

namespace TravelBuddy.Calificaciones
{
    [Authorize]
    public class CalificacionAppService :
        CrudAppService<
            Calificacion,
            CalificacionDTO,
            Guid,
            GetCalificacionesInput,
            crearCalificacionDTO>,
        ICalificacionAppService
    {
        private readonly IRepository<Calificacion, Guid> _calificacionRepository;

        public CalificacionAppService(IRepository<Calificacion, Guid> repository)
            : base(repository)
        {
            _calificacionRepository = repository;
        }

        protected override async Task<IQueryable<Calificacion>> CreateFilteredQueryAsync(GetCalificacionesInput input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            if (input.DestinoId.HasValue)
            {
                query = query.Where(x => x.DestinoId == input.DestinoId);
            }
            return query;
        }

        [AllowAnonymous]
        public override Task<PagedResultDto<CalificacionDTO>> GetListAsync(GetCalificacionesInput input)
        {
            return base.GetListAsync(input);
        }

        [AllowAnonymous]
        public async Task<double> GetPromedioAsync(Guid destinoId)
        {
            var query = await _calificacionRepository.GetQueryableAsync();
            var calificaciones = query.Where(x => x.DestinoId == destinoId);

            if (!calificaciones.Any()) return 0;
            return calificaciones.Average(x => x.Puntaje);
        }

        public override async Task<CalificacionDTO> CreateAsync(crearCalificacionDTO input)
        {
            if (CurrentUser.Id == null)
            {
                throw new AbpAuthorizationException("No estás autorizado");
            }

            var existingRating = await _calificacionRepository.FirstOrDefaultAsync(
                c => c.UserId == CurrentUser.Id && c.DestinoId == input.DestinoId
            );

            if (existingRating != null)
            {
                throw new UserFriendlyException("Ya has calificado este destino.");
            }

            var calificacion = new Calificacion(
                GuidGenerator.Create(),
                input.DestinoId,
                CurrentUser.Id.Value,
                input.Puntaje,
                input.Comentario
            );

            await _calificacionRepository.InsertAsync(calificacion);

            return ObjectMapper.Map<Calificacion, CalificacionDTO>(calificacion);
        }

        public override async Task<CalificacionDTO> UpdateAsync(Guid id, crearCalificacionDTO input)
        {
            var calificacion = await _calificacionRepository.GetAsync(id);
            ValidarPropiedad(calificacion);

            calificacion.Actualizar(input.Puntaje, input.Comentario);
            await _calificacionRepository.UpdateAsync(calificacion);

            return ObjectMapper.Map<Calificacion, CalificacionDTO>(calificacion);
        }

        public override async Task DeleteAsync(Guid id)
        {
            var calificacion = await _calificacionRepository.GetAsync(id);
            ValidarPropiedad(calificacion);

            await _calificacionRepository.DeleteAsync(calificacion);
        }

        private void ValidarPropiedad(Calificacion calificacion)
        {
            if (calificacion.UserId != CurrentUser.GetId())
            {
                throw new AbpAuthorizationException("Solo puedes modificar tus propias calificaciones.");
            }
        }
    }
}