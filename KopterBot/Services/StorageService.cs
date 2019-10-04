using KopterBot.DTO;
using KopterBot.Repository;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class StorageService :RepositoryProvider
    {
        public async Task Create(StorageDTO storage)
        {
            await storageRepository.Create(storage);
        }
    }
}
