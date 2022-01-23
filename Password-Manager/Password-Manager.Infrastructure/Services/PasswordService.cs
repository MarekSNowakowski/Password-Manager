using Password_Manager.Core.Domain;
using Password_Manager.Core.Repositories;
using Password_Manager.Infrastructure.Commands;
using Password_Manager.Infrastructure.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Password_Manager.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository;

        public PasswordService(IPasswordRepository passwordRepository)
        {
            _passwordRepository = passwordRepository;
        }

        private IEnumerable<PasswordDTO> MapPasswords(IEnumerable<Password> passwords)
        {
            return passwords.Select(x => new PasswordDTO()
            {
                Id = x.Id,
                Service = x.Service,
                Username = x.Username,
                PassEncrypted = x.PassEncrypted,
                Salt = x.Salt,
                Author = x.Author
            });
        }

        private PasswordDTO MapPassword(Password password)
        {
            var passwordDTO = new PasswordDTO()
            {
                Id = password.Id,
                Service = password.Service,
                Username = password.Username,
                PassEncrypted = password.PassEncrypted,
                Salt = password.Salt,
                Author = password.Author
            };

            return passwordDTO;
        }

        public async Task<IEnumerable<PasswordDTO>> BrowseAllAsync()
        {
            var z = await _passwordRepository.BrowseAllAsync();
            if (z != null)
                return MapPasswords(z);
            else
                return null;
        }

        public async Task<PasswordDTO> GetPasswordAsync(int id)
        {
            Password z = await _passwordRepository.GetAsync(id);
            if (z != null)
                return MapPassword(z);
            else
                return null;
        }

        public async Task AddPasswordAsync(CreatePassword password)
        {
            Password newPassword = new Password()
            {
                Service = password.Service,
                Username = password.Username,
                PassEncrypted = password.PassEncrypted,
                Salt = password.Salt,
                Author = password.Author
            };

            await _passwordRepository.AddAsync(newPassword);
        }

        public async Task DeletePasswordAsync(int id)
        {
            await _passwordRepository.DelAsync(id);
        }

        public async Task EditPasswordAsync(int id, CreatePassword password)
        {
            Password updatePassword = await _passwordRepository.GetAsync(id);
            updatePassword.Service = password.Service;
            updatePassword.Username = password.Username;
            updatePassword.PassEncrypted = password.PassEncrypted;
            updatePassword.Salt = password.Salt;

            await _passwordRepository.UpdateAsync(updatePassword);
        }
    }
}
