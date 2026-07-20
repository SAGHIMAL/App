using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using TravelBuddy.Experiencias;
namespace TravelBuddy.Experiencias
{
    [Authorize]
    public class ExperienciaAppService :
        CrudAppService<
            Experiencia,
            ExperienciaDTO,
            Guid,
            GetExperienciasInput,
            CrearExperienciaDTO>,
        IExperienciaAppService
    {
        private readonly IRepository<Experiencia, Guid> _experienciaRepository;

        public ExperienciaAppService(IRepository<Experiencia, Guid> repository)
            : base(repository)
        {
            _experienciaRepository = repository;
        }

       
        protected override async Task<IQueryable<Experiencia>> CreateFilteredQueryAsync(GetExperienciasInput input)
        {
            
            var query = await base.CreateFilteredQueryAsync(input);

           
            if (input.CityID.HasValue)
            {
                query = query.Where(x => x.CityID == input.CityID);
            }

           
            if (input.Valoracion.HasValue)
            {
                query = query.Where(x => x.Valoracion == input.Valoracion);
            }

            
            if (!string.IsNullOrEmpty(input.TextoFiltrado))
            {
                query = query.Where(x =>
                    x.Titulo.Contains(input.TextoFiltrado) ||
                    x.Descripcion.Contains(input.TextoFiltrado));
            }

            return query;
        }

        public override async Task<ExperienciaDTO> CreateAsync(CrearExperienciaDTO input)
        {
            var experiencia = new Experiencia(
                GuidGenerator.Create(),
                input.Titulo,
                input.Descripcion,
                input.CityID,
                CurrentUser.GetId(),
                input.FechaExperiencia,
                input.Valoracion
            );

            await _experienciaRepository.InsertAsync(experiencia);

            return ObjectMapper.Map<Experiencia, ExperienciaDTO>(experiencia);
        }

        public override async Task<ExperienciaDTO> UpdateAsync(Guid id, CrearExperienciaDTO input)
        {
            var experiencia = await _experienciaRepository.GetAsync(id);
            ValidarPropiedad(experiencia);

            experiencia.Titulo = input.Titulo;
            experiencia.Descripcion = input.Descripcion;
            experiencia.FechaExperiencia = input.FechaExperiencia;
            experiencia.Valoracion = input.Valoracion;
            await _experienciaRepository.UpdateAsync(experiencia);

            return ObjectMapper.Map<Experiencia, ExperienciaDTO>(experiencia);
        }

        public override async Task DeleteAsync(Guid id)
        {
            var experiencia = await _experienciaRepository.GetAsync(id);
            ValidarPropiedad(experiencia);

            await _experienciaRepository.DeleteAsync(experiencia);
        }

        private void ValidarPropiedad(Experiencia experiencia)
        {
            if (experiencia.UserId != CurrentUser.GetId())
            {
                throw new AbpAuthorizationException("Solo puedes modificar tus propias experiencias.");
            }
        }
    }
}