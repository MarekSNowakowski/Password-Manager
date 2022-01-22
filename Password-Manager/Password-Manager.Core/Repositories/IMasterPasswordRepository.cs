using Password_Manager.Core.Domain;
using System.Threading.Tasks;

namespace Password_Manager.Core.Repositories
{
    public interface IMasterPasswordRepository
    {
        public Task<MasterPassword> GetAsync(string username);
        public Task<MasterPassword> GetAsync(int id);
        public Task AddAsync(MasterPassword s);
        public Task DelAsync(int id);
        public Task UpdateAsync(MasterPassword s);
    }
}
