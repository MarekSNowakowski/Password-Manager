using System.ComponentModel.DataAnnotations;

namespace Password_Manager.WebApp.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana!")]
        [Display(Name = "Nazwa Użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Master password jest wymagane!")]
        [Display(Name = "Master password")]
        [DataType(DataType.Password)]
        public string MasterPassword { get; set; }
    }
}
