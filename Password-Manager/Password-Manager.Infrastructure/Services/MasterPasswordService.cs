using Password_Manager.Core.Domain;
using Password_Manager.Core.Repositories;
using Password_Manager.Infrastructure.Commands;
using Password_Manager.Infrastructure.DTO;
using System.Threading.Tasks;

namespace Password_Manager.Infrastructure.Services
{
    public class MasterPasswordService : IMasterPasswordService
    {
        private readonly IMasterPasswordRepository _masterPasswordRepository;

        public MasterPasswordService(IMasterPasswordRepository masterPasswordRepository)
        {
            _masterPasswordRepository = masterPasswordRepository;
        }

        private MasterPasswordDTO MapMasterPassword(MasterPassword masterPassword)
        {
            var passwordDTO = new MasterPasswordDTO()
            {
                Id = masterPassword.Id,
                Username = masterPassword.Username,
                MasterPasswordHash = masterPassword.MasterPasswordHash
            };

            return passwordDTO;
        }

        public async Task<MasterPasswordDTO> GetMasterPasswordAsync(string username)
        {
            MasterPassword z = await _masterPasswordRepository.GetAsync(username);
            if (z != null)
                return MapMasterPassword(z);
            else
                return null;
        }

        public async Task AddMasterPasswordAsync(CreateMasterPassword masterPassword)
        {
            MasterPassword newMasterPassword = new MasterPassword()
            {
                Username = masterPassword.Username,
                MasterPasswordHash = masterPassword.MasterPasswordHash
            };

            await _masterPasswordRepository.AddAsync(newMasterPassword);
        }

        public async Task DeleteMasterPasswordAsync(int id)
        {
            await _masterPasswordRepository.DelAsync(id);
        }

        public async Task EditMasterPasswordAsync(int id, CreateMasterPassword masterPassword)
        {
            MasterPassword updateMasterPassword = await _masterPasswordRepository.GetAsync(id);
            updateMasterPassword.Username = masterPassword.Username;
            updateMasterPassword.MasterPasswordHash = masterPassword.MasterPasswordHash;

            await _masterPasswordRepository.UpdateAsync(updateMasterPassword);
        }
    }
}
