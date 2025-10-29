using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBuddy.Ciudades
{
    public interface ICitySearchService
    {
        Task<List<CiudadesExternasDTO>> SearchByNameAsync(string nombreParcial);
    }
}
