using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Destinos;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace TravelBuddy.Seguimientos
{
    [Authorize]
    public class SeguimientoAppService : TravelBuddyAppService, ISeguimientoAppService
    {
        private readonly IRepository<DestinoSeguido, Guid> _seguimientoRepository;
        private readonly IRepository<Destino, Guid> _destinoRepository;
        private readonly ICurrentUser _currentUser;

        public SeguimientoAppService(
            IRepository<DestinoSeguido, Guid> seguimientoRepository,
            IRepository<Destino, Guid> destinoRepository,
            ICurrentUser currentUser)
        {
            _seguimientoRepository = seguimientoRepository;
            _destinoRepository = destinoRepository;
            _currentUser = currentUser;
        }

        public async Task AgregarAsync(Guid destinoId)
        {
            if (!_currentUser.Id.HasValue)
            {
                throw new AbpAuthorizationException("No estás autorizado");
            }

            var destino = await _destinoRepository.FindAsync(destinoId);

            if (destino == null)
            {
                throw new UserFriendlyException("El destino no existe.");
            }

            var yaSeguido = await _seguimientoRepository.FirstOrDefaultAsync(
                s => s.UserId == _currentUser.Id && s.DestinoId == destinoId
            );

            if (yaSeguido != null)
            {
                throw new UserFriendlyException("Este destino ya está en tu lista de seguimiento.");
            }

            var seguimiento = new DestinoSeguido(
                GuidGenerator.Create(),
                destinoId,
                _currentUser.Id.Value
            );

            await _seguimientoRepository.InsertAsync(seguimiento);
        }

        public async Task EliminarAsync(Guid destinoId)
        {
            if (!_currentUser.Id.HasValue)
            {
                throw new AbpAuthorizationException("No estás autorizado");
            }

            var seguimiento = await _seguimientoRepository.FirstOrDefaultAsync(
                s => s.UserId == _currentUser.Id && s.DestinoId == destinoId
            );

            if (seguimiento == null)
            {
                throw new UserFriendlyException("Este destino no está en tu lista de seguimiento.");
            }

            await _seguimientoRepository.DeleteAsync(seguimiento);
        }

        public async Task<List<DestinoSeguidoDTO>> GetMiListaAsync()
        {
            if (!_currentUser.Id.HasValue)
            {
                throw new AbpAuthorizationException("No estás autorizado");
            }

            var seguimientos = await _seguimientoRepository.GetQueryableAsync();
            var destinos = await _destinoRepository.GetQueryableAsync();

            var query =
                from s in seguimientos
                where s.UserId == _currentUser.Id.Value
                join d in destinos on s.DestinoId equals d.Id
                orderby s.CreationTime descending
                select new DestinoSeguidoDTO
                {
                    Id = s.Id,
                    DestinoId = d.Id,
                    Ciudad = d.Ciudad,
                    Pais = d.Pais,
                    Coordenadas = d.Coordenadas,
                    Foto = d.Foto,
                    Poblacion = d.Poblacion,
                    FechaAgregado = s.CreationTime,
                    UltimaActualizacion = d.LastModificationTime ?? d.CreationTime
                };

            return await AsyncExecuter.ToListAsync(query);
        }
    }
}
