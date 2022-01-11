using System;
using System.Collections.Generic;
using System.Text;

namespace Password_Manager.Infrastructure.DTO
{
    public class PasswordDTO
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public string Pass { get; set; }
        public string Author { get; set; }
    }
}
