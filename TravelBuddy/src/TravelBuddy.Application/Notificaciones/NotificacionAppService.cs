using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TravelBuddy.Notificaciones;
using Volo.Abp.Application.Services;    
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

    [Authorize]
    public class NotificacionAppService : ApplicationService, INotificacionAppService
    {
        private readonly IRepository<Notificacion, Guid> _notificacionRepository;
       
        private readonly IRepository<PreferenciaNotificacion, Guid> _preferenciaRepository;
        public NotificacionAppService(IRepository<Notificacion, Guid> notificacionRepository, IRepository<PreferenciaNotificacion, Guid> preferenciaRepository  )
        {
            _notificacionRepository = notificacionRepository;
            _preferenciaRepository = preferenciaRepository;
        }
        public async Task CrearNotificacionPruebaAsync(string mensaje, Guid? destinoId)
        {
            var userId = CurrentUser.GetId();
            var notificacion = new Notificacion (GuidGenerator.Create(), userId, mensaje, destinoId);
            await _notificacionRepository.InsertAsync(notificacion);
        }
        public async Task MarcarComoLeidaAsync(Guid id)
        {
            var notificacion = await _notificacionRepository.GetAsync(id);
            notificacion.Leida = true;
            await _notificacionRepository.UpdateAsync(notificacion);
        }
        public async Task<List<NotificacionDTO>> GetNotificacionesASync()
        {
            var userId = CurrentUser.GetId();
            var notificaciones = await _notificacionRepository.GetListAsync(n => n.UserId == userId);
            return ObjectMapper.Map<List<Notificacion>, List<NotificacionDTO>>(notificaciones);
        }

        public async Task<PreferenciaNotificacionDTO> GetMisPreferenciasAsync()
        {
            var userId = CurrentUser.GetId();
            var pref = await _preferenciaRepository.FirstOrDefaultAsync(x => x.UserId == userId);

            if (pref == null)
            {
                // Si no configuró nada, devolvemos los defaults (todo activado)
                return new PreferenciaNotificacionDTO
                {
                    NotificarComentariosEnFavoritos = true,
                    NotificarEventosDestinos = true
                };
            }

            return ObjectMapper.Map<PreferenciaNotificacion, PreferenciaNotificacionDTO>(pref);
        }

        public async Task UpdateMisPreferenciasAsync(PreferenciaNotificacionDTO input)
        {
            var userId = CurrentUser.GetId();
            var pref = await _preferenciaRepository.FirstOrDefaultAsync(x => x.UserId == userId);

            if (pref == null)
            {
                // Si no existe, la creamos
                pref = new PreferenciaNotificacion(GuidGenerator.Create(), userId);
                await _preferenciaRepository.InsertAsync(pref);
            }

            // Actualizamos los valores
            pref.NotificarComentariosEnFavoritos = input.NotificarComentariosEnFavoritos;
            pref.NotiificarEventosDestinos = input.NotificarEventosDestinos;

            await _preferenciaRepository.UpdateAsync(pref);
        }
    }

