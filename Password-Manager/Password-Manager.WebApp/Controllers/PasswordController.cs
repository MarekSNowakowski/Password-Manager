using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Password_Manager.Infrastructure.Services;
using Password_Manager.WebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.WebApp.Controllers
{
    public class PasswordController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PasswordController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //AES
        private readonly byte[] IV = Encoding.ASCII.GetBytes("OB8JQ5QREz7SLup6");
        //PBKDF2
        public static readonly byte[] SALT = Encoding.ASCII.GetBytes("67r1m*Sk8os=U4-D*mtB2DsP"); // size in bytes
        public const int KEY_SIZE = 24; // size in bytes
        public const int ITERATIONS = 100000; // number of pbkdf2 iterations

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

        public IActionResult Index(bool wrongPassword = false)
        {
            if (wrongPassword)
                ModelState.AddModelError("", "Niepoprawne hasło główne!");
            return View(new MasterPasswordVM());
        }

        [HttpPost]
        public async Task<IActionResult> PasswordList(string masterPassword)
        {
            // Check if master password is correct
            if (masterPassword!= null && !await VerifyMasterPassword(masterPassword))
            {
                return RedirectToAction(nameof(Index), new { wrongPassword = true });
            }

            string _restpath = GetHostUrl().Content + CN();

            List<PasswrodVM> passwordsList = new List<PasswrodVM>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        passwordsList = JsonConvert.DeserializeObject<List<PasswrodVM>>(apiResponse)
                            .FindAll(x=>x.Author == User.Identity.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Getting posts failed!");
                return View(ex);
            }


            foreach (PasswrodVM pass in passwordsList)
            {
                pass.Pass = DecryptStringFromBytes_Aes(pass.PassEncrypted, masterPassword, IV);
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
            s.PassEncrypted = EncryptStringToBytes_Aes(s.Pass, s.MasterPassword, IV);
            s.MasterPassword = null;

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

        static byte[] EncryptStringToBytes_Aes(string password, string masterPassword, byte[] IV)
        {
            // Check arguments.
            if (password == null || password.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (masterPassword == null || masterPassword.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] masterPassBytes = Encoding.ASCII.GetBytes(masterPassword);
            byte[] encrypted;

            // Generate the key
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(masterPassBytes, SALT, ITERATIONS);
            byte[] key = pbkdf2.GetBytes(KEY_SIZE);

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(password);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, string masterPassword, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (masterPassword == null || masterPassword.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] masterPassBytes = Encoding.ASCII.GetBytes(masterPassword);

            // Generate the key
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(masterPassBytes, SALT, ITERATIONS);
            byte[] key = pbkdf2.GetBytes(KEY_SIZE);

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private async Task<bool> VerifyMasterPassword(string masterPassword)
        {
            byte[] hash = await GetMasterPasswordHash();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] hashed = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(masterPassword));

                bool bEqual = false;

                if (hashed.Length == hash.Length)
                {
                    int i = 0;
                    while ((i < hashed.Length) && (hashed[i] == hash[i]))
                    {
                        i += 1;
                    }
                    if (i == hashed.Length)
                    {
                        bEqual = true;
                    }
                }

                return bEqual;
            }
        }

        private async Task<byte[]> GetMasterPasswordHash()
        {
            string _restpath = GetHostUrl().Content + "masterpassword/" + User.Identity.Name;

            MasterPasswordVM masterPassword = new MasterPasswordVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        masterPassword = JsonConvert.DeserializeObject<MasterPasswordVM>(apiResponse);
                    }
                }
            }
            catch
            {
                _logger.LogWarning($"Getting posts failed!");
            }

            return masterPassword.MasterPasswordHash;
        }
    }
}
