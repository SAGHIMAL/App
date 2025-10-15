using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace TravelBuddy.Destinos;

    public interface IDestinoAppService: 
    ICrudAppService < 
        destinoDTO, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdatedestinoDTO>    
{

    }

