using Password_Manager.Infrastructure.Commands;
using Password_Manager.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Password_Manager.Infrastructure.Services
{
    public interface IPasswordService
    {
        Task<IEnumerable<PasswordDTO>> BrowseAllAsync();
        Task<PasswordDTO> GetPasswordAsync(int id);
        Task AddPasswordAsync(CreatePassword post);
        Task EditPasswordAsync(int id, CreatePassword post);
        Task DeletePasswordAsync(int id);
    }
}
