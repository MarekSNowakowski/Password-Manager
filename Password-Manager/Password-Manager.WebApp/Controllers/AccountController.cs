using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Password_Manager.WebApp.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Password_Manager.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<PasswordController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private const string CN = "masterpassword";

        public AccountController(IConfiguration configuration, SignInManager<IdentityUser> signInManager, ILogger<PasswordController> logger, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public ContentResult GetHostUrl()
        {
            var result = _configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        public IActionResult Login()
        {
            return View();
        }

        //Loging POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            // returns user name to login
            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if(user != null)
            {
                // login try
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło...");

            return View(loginVM);
        }

        public IActionResult Register()
        {
            return View(new LoginVM());
        }

        //Registering POST
        [HttpPost]
        public async Task<IActionResult> Register(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = loginVM.UserName };

                MasterPasswordVM masterPasswordVM = new MasterPasswordVM()
                {
                    Username = loginVM.UserName,
                    MasterPasswordHash = HashMasterPassword(loginVM.MasterPassword)
                };

                AddMasterPassword(masterPasswordVM);

                var result = await _userManager.CreateAsync(user, loginVM.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");   //(metoda, controller)
                }
            }


            ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło...");

            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async void AddMasterPassword(MasterPasswordVM masterPasswordVM)
        {
            string _restpath = GetHostUrl().Content + CN;

            MasterPasswordVM result = new MasterPasswordVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(masterPasswordVM);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync($"{_restpath}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<MasterPasswordVM>(apiResponse);
                    }
                }

            }
            catch
            {
                _logger.LogWarning($"Creating master password failed!");
            }

            _logger.LogInformation($"Creating  master password for user: {result.Username} succeded!");
        }

        private byte[] HashMasterPassword(string masterPassword)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] hashed = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(masterPassword));

                return hashed;
            }
        }
    }
}
