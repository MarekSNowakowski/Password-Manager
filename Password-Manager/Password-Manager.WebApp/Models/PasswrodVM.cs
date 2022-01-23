using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Password_Manager.WebApp.Models
{
    public class PasswrodVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa serwisu jest wymagana!")]
        [Display(Name = "Nazwa Serwisu")]
        public string Service { get; set; }

        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

        public string Author { get; set; }

        public byte[] PassEncrypted { get; set; }

        public byte[] Salt { get; set; }

        // Only for adding new password
        [Required(ErrorMessage = "Master password jest wymagane!")]
        [Display(Name = "Master password")]
        [DataType(DataType.Password)]
        public string MasterPassword { get; set; }

        // For verification of master password
        public string MasterPasswordHash { get; set; }
    }
}
