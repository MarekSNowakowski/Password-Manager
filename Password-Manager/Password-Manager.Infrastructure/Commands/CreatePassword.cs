using System;
using System.Collections.Generic;
using System.Text;

namespace Password_Manager.Infrastructure.Commands
{
    public class CreatePassword
    {
        public string Service { get; set; }
        public string Pass { get; set; }
        public string Username { get; set; }
        public string Author { get; set; }
    }
}
