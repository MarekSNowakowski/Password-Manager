using System.ComponentModel.DataAnnotations;

namespace Password_Manager.WebApp.Models
{
    public class MasterPasswordVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] MasterPasswordHash { get; set; }

        [Required(ErrorMessage = "Master password jest wymagane!")]
        [Display(Name = "Master password")]
        [DataType(DataType.Password)]
        public string MasterPassword { get; set; }
    }
}
