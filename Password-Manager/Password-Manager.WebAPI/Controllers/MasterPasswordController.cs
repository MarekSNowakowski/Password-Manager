using Microsoft.AspNetCore.Mvc;
using Password_Manager.Infrastructure.Commands;
using Password_Manager.Infrastructure.Services;
using System.Threading.Tasks;

namespace Password_Manager.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class MasterPasswordController : Controller
    {
        private readonly IMasterPasswordService _materPasswordRepository;

        public MasterPasswordController(IMasterPasswordService passwordRepository)
        {
            _materPasswordRepository = passwordRepository;
        }

        // masterpassword/{username}
        [HttpGet("{username}")]
        public async Task<IActionResult> GetMasterPassword(string username)
        {
            var z = await _materPasswordRepository.GetMasterPasswordAsync(username);
            return Json(z);
        }

        // masterpassword
        [HttpPost]
        public async Task<IActionResult> AddMasterPassword([FromBody] CreateMasterPassword masterPassword)
        {
            await _materPasswordRepository.AddMasterPasswordAsync(masterPassword);
            return Created("", masterPassword);
        }

        // masterpassword/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPassword(int id, [FromBody] CreateMasterPassword masterPassword)
        {
            await _materPasswordRepository.EditMasterPasswordAsync(id, masterPassword);
            return NoContent();
        }

        // masterpassword/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterPassword(int id)
        {
            await _materPasswordRepository.DeleteMasterPasswordAsync(id);
            return NoContent();
        }
    }
}

