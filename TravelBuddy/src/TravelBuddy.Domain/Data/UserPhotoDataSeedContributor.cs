using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Guids;

namespace TravelBuddy.Data
{
    public class UserPhotoDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityUserManager _userManager;

        public UserPhotoDataSeedContributor(
            IIdentityUserRepository userRepository,
            IdentityUserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // 1. Buscamos al usuario admin por su username
            var adminUser = await _userRepository.FindByNormalizedUserNameAsync("ADMIN");

            if (adminUser != null)
            {
                // 2. Verificamos si ya tiene foto para no sobrescribir
                var fotoActual = adminUser.GetProperty<string>("Foto");

                if (string.IsNullOrEmpty(fotoActual))
                {
                    // 3. Le asignamos una URL de ejemplo o un base64
                    /* Puedes usar una URL pública para probar, 
                       o dejarlo vacío si solo quieres que el campo exista.
                    */
                    string defaultPhotoUrl = "https://www.shutterstock.com/shutterstock/photos/2603763193/display_1500/stock-photo-business-creative-and-portrait-of-black-man-in-studio-for-startup-company-entrepreneurship-and-2603763193.jpg";

                    adminUser.SetProperty("Foto", defaultPhotoUrl);

                    // 4. Guardamos los cambios
                    await _userManager.UpdateAsync(adminUser);
                }
            }
        }
    }
}