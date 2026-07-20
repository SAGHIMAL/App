using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TravelBuddy.Administracion
{
    public interface IMetricasApiExternaAppService : IApplicationService
    {
        Task<MetricasApiExternaDTO> GetMetricasAsync();
    }
}
