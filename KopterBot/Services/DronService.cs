using KopterBot.DTO;
using KopterBot.Repository;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class DronService : RepositoryProvider
    {
        public async Task Create(DronDTO dron)
        {
            await dronRepository.Create(dron);
        }
    }
}
