using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBuddy.Favoritos;
using TravelBuddy.Notificaciones;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Authorization;

namespace TravelBuddy.Comentarios
{
    [Authorize]
    public class ComentarioAppService : ApplicationService, IComentarioAppService
    {
        private readonly IRepository<Comentario, Guid> _comentarioRepository;
        private readonly IRepository<Favorito, Guid> _favoritoRepository;
        private readonly IRepository<Notificacion, Guid> _notificacionRepository;
        private readonly IRepository<PreferenciaNotificacion, Guid> _preferenciaRepository;

        public ComentarioAppService(
            IRepository<Comentario, Guid> comentarioRepository,
            IRepository<Favorito, Guid> favoritoRepository,
            IRepository<Notificacion, Guid> notificacionRepository,
            IRepository<PreferenciaNotificacion, Guid> preferenciaRepository)
        {
            _comentarioRepository = comentarioRepository;
            _favoritoRepository = favoritoRepository;
            _notificacionRepository = notificacionRepository;
            _preferenciaRepository = preferenciaRepository;
        }

        public async Task CreateAsync(CrearComentarioDTO input)
        {
            var currentUserId = CurrentUser.GetId();

            var comentario = new Comentario(
                GuidGenerator.Create(),
                input.DestinoId,
                currentUserId,
                input.Texto
            );
            await _comentarioRepository.InsertAsync(comentario);

            var fans = await _favoritoRepository.GetListAsync(f => f.DestinoId == input.DestinoId);

            foreach (var fan in fans)
            {
                if (fan.UserId == currentUserId) continue;

                var preferencia = await _preferenciaRepository.FirstOrDefaultAsync(p => p.UserId == fan.UserId);

                bool debeNotificar = preferencia == null || preferencia.NotificarComentariosEnFavoritos;

                if (debeNotificar)
                {
                    
                    var notificacion = new Notificacion(
                        GuidGenerator.Create(),
                        fan.UserId,
                        $"Nuevo comentario: {input.Texto.Substring(0, Math.Min(input.Texto.Length, 20))}...",
                   
                        input.DestinoId
                    );

                    await _notificacionRepository.InsertAsync(notificacion);
                }
            }
        }

        [AllowAnonymous]
        public async Task<List<ComentarioDTO>> GetListPorDestinoAsync(Guid destinoId)
        {
            var comentarios = await _comentarioRepository.GetListAsync(c => c.DestinoId == destinoId);

            return ObjectMapper.Map<List<Comentario>, List<ComentarioDTO>>(
                comentarios.OrderByDescending(c => c.CreationTime).ToList()
            );
        }
    }
}