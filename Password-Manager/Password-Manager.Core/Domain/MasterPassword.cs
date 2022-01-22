namespace Password_Manager.Core.Domain
{
    public class MasterPassword
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] MasterPasswordHash { get; set; }
    }
}
