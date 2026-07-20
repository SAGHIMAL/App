using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace TravelBuddy.ApisExternas
{
    public class RegistroLlamadaApi : CreationAuditedEntity<Guid>
    {
        public string NombreApi { get; private set; }
        public string Endpoint { get; private set; }
        public bool Exitosa { get; private set; }

        private RegistroLlamadaApi() { }

        public RegistroLlamadaApi(Guid id, string nombreApi, string endpoint, bool exitosa)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(nombreApi))
            {
                throw new ArgumentException("El nombre de la API es obligatorio.", nameof(nombreApi));
            }
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("El endpoint es obligatorio.", nameof(endpoint));
            }

            NombreApi = nombreApi;
            Endpoint = endpoint;
            Exitosa = exitosa;
        }
    }
}
