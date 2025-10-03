using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;    
using Volo.Abp.Application.Dtos;        
using Volo.Abp.Domain.Repositories;

namespace TravelBuddy.Destinos;

    public class DestinoAppService : 
       CrudAppService < 
        Destino, 
        destinoDTO, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdatedestinoDTO>,
        IDestinoAppService
{
    public DestinoAppService(IRepository<Destino, Guid> repository)
        : base(repository)
    {  
        
    }
}   

