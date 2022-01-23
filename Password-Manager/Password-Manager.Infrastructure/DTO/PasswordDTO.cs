namespace Password_Manager.Infrastructure.DTO
{
    public class PasswordDTO
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public string Username { get; set; }
        public byte[] PassEncrypted { get; set; }
        public byte[] Salt { get; set; }
        public string Author { get; set; }
    }
}
