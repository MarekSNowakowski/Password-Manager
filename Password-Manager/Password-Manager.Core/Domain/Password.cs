﻿namespace Password_Manager.Core.Domain
{
    public class Password
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public string Username { get; set; }
        public byte[] PassEncrypted { get; set; }
        public byte[] Salt { get; set; }
        public string Author { get; set; }
    }
}
