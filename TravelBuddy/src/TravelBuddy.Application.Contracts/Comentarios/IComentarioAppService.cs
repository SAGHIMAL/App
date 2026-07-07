using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;    

namespace TravelBuddy.Comentarios
{
    public interface IComentarioAppService : IApplicationService
    {
        Task CreateAsync(CrearComentarioDTO input);
    }
}
