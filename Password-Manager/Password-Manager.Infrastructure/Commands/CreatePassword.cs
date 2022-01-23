namespace Password_Manager.Infrastructure.Commands
{
    public class CreatePassword
    {
        public string Service { get; set; }
        public byte[] PassEncrypted { get; set; }
        public string Username { get; set; }
        public string Author { get; set; }
    }
}
