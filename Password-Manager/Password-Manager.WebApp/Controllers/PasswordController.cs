using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Password_Manager.Infrastructure.Services;
using Password_Manager.WebApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.WebApp.Controllers
{
    public class PasswordController : Controller
    {
        private readonly IPasswordService _passwordRepository;

        private readonly IConfiguration _configuration;
        private readonly ILogger<PasswordController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PasswordController(IConfiguration configuration, ILogger<PasswordController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public ContentResult GetHostUrl()
        {
            var result = _configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private string CN()
        {
            string cn = ControllerContext.RouteData.Values["controller"].ToString();
            return cn;
        }

        public async Task<IActionResult> Index()
        {
            //string _restpath = "http://localhost:5000/skijumper";
            string _restpath = GetHostUrl().Content + CN();

            List<PasswrodVM> passwordsList = new List<PasswrodVM>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        passwordsList = JsonConvert.DeserializeObject<List<PasswrodVM>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting posts failed!");
                return View(ex);
            }

            return View(passwordsList);    //view is strongly typed
        }

        public async Task<IActionResult> Edit(int id)
        {
            string _restpath = GetHostUrl().Content + CN();

            PasswrodVM s = new PasswrodVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        s = JsonConvert.DeserializeObject<PasswrodVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Editing password with ID {id} failed!");
                return View(ex);
            }

            return View(s);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PasswrodVM s)
        {
            string _restpath = GetHostUrl().Content + CN();

            PasswrodVM result = new PasswrodVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync($"{_restpath}/{s.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PasswrodVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Editing password with ID {s.Id} failed!");
                return View(ex);
            }

            _logger.LogInformation($"Editing password with ID {s.Id} succeded!");
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Deleting password with ID {id} failed!");
                return View(ex);
            }

            _logger.LogInformation($"Deleting password with ID: {id} succeded!");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            PasswrodVM s = new PasswrodVM();
            return await Task.FromResult(View(s));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PasswrodVM s)
        {
            string _restpath = GetHostUrl().Content + CN();

            s.Author = User.Identity.Name;

            PasswrodVM result = new PasswrodVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync($"{_restpath}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PasswrodVM>(apiResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Creating post failed!");
                return View(ex);
            }

            _logger.LogInformation($"Creating password with ID: {result.Id} succeded!");
            return RedirectToAction(nameof(Index));
        }
    }
}
