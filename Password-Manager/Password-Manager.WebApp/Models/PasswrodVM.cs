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

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

        public string Author { get; set; }
    }
}
