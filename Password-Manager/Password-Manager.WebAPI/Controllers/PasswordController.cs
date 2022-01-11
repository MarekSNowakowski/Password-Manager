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

        // post
        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var z = await _passwordRepository.BrowseAllAsync();
            return Json(z);
        }

        // post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPassword(int id)
        {
            var z = await _passwordRepository.GetPasswordAsync(id);
            return Json(z);
        }

        // post
        [HttpPost]
        public async Task<IActionResult> AddPassword([FromBody] CreatePassword post)
        {
            await _passwordRepository.AddPasswordAsync(post);
            return Created("", post);
        }

        // post/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPassword(int id, [FromBody] CreatePassword post)
        {
            await _passwordRepository.EditPasswordAsync(id, post);
            return NoContent();
        }

        // post/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _passwordRepository.DeletePasswordAsync(id);
            return NoContent();
        }
    }
}

