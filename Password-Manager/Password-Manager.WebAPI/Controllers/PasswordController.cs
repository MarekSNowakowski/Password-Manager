using Microsoft.AspNetCore.Mvc;
using Password_Manager.Infrastructure.Commands;
using Password_Manager.Infrastructure.Services;
using System.Threading.Tasks;

namespace Password_Manager.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class PasswordController : Controller
    {
        private readonly IPasswordService _passwordRepository;

        public PasswordController(IPasswordService passwordRepository)
        {
            _passwordRepository = passwordRepository;
        }

        // password
        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var z = await _passwordRepository.BrowseAllAsync();
            return Json(z);
        }

        // password/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPassword(int id)
        {
            var z = await _passwordRepository.GetPasswordAsync(id);
            return Json(z);
        }

        // password
        [HttpPost]
        public async Task<IActionResult> AddPassword([FromBody] CreatePassword password)
        {
            await _passwordRepository.AddPasswordAsync(password);
            return Created("", password);
        }

        // password/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPassword(int id, [FromBody] CreatePassword password)
        {
            await _passwordRepository.EditPasswordAsync(id, password);
            return NoContent();
        }

        // password/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(int id)
        {
            await _passwordRepository.DeletePasswordAsync(id);
            return NoContent();
        }
    }
}

