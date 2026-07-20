using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TravelBuddy.Experiencias;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace TravelBuddy.Perfiles
{
    [Authorize]
    public class PerfilAppService : ApplicationService, IPerfilAppService
    {
        private readonly IdentityUserManager _identityUserManager;
        private readonly IRepository<Experiencia, Guid> _experienciaRepository;

        public PerfilAppService(
            IdentityUserManager identityUserManager,
            IRepository<Experiencia, Guid> experienciaRepository)
        {
            _identityUserManager = identityUserManager;
            _experienciaRepository = experienciaRepository;
        }

        public async Task EliminarMiCuentaAsync()
        {
            if (CurrentUser.Id == null)
            {
                throw new Volo.Abp.Authorization.AbpAuthorizationException("No estás autorizado");
            }

            var userId = CurrentUser.GetId();
            var user = await _identityUserManager.GetByIdAsync(userId);

            var result = await _identityUserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new UserFriendlyException("No se pudo eliminar la cuenta: " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        [AllowAnonymous]
        public async Task<PerfilPublicoDTO> GetPerfilPublicoAsync(Guid userId)
        {
            var user = await _identityUserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UserFriendlyException("El usuario no existe.");
            }

            var experiencias = await _experienciaRepository.GetListAsync(e => e.UserId == userId);

            return new PerfilPublicoDTO
            {
                UserId = user.Id,
                UserName = user.UserName,
                Nombre = user.Name,
                Apellido = user.Surname,
                Foto = user.GetProperty<string?>("Foto"),
                Experiencias = ObjectMapper.Map<List<Experiencia>, List<ExperienciaDTO>>(
                    experiencias.OrderByDescending(e => e.FechaExperiencia).ToList())
            };
        }
    }
}
