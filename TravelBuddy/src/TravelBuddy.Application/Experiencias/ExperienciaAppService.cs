using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using TravelBuddy.Experiencias;
namespace TravelBuddy.Experiencias
{
    public class ExperienciaAppService :
        CrudAppService<
            Experiencia,
            ExperienciaDTO,
            Guid,
            GetExperienciasInput, 
            CrearExperienciaDTO>,
        IExperienciaAppService
    {
        public ExperienciaAppService(IRepository<Experiencia, Guid> repository)
            : base(repository)
        {
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
    }
}