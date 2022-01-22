using Password_Manager.Infrastructure.Commands;
using Password_Manager.Infrastructure.DTO;
using System.Threading.Tasks;

namespace Password_Manager.Infrastructure.Services
{
    public interface IMasterPasswordService
    {
        Task<MasterPasswordDTO> GetMasterPasswordAsync(string username);
        Task AddMasterPasswordAsync(CreateMasterPassword masterPassword);
        Task EditMasterPasswordAsync(int id, CreateMasterPassword masterPassword);
        Task DeleteMasterPasswordAsync(int id);
    }
}
