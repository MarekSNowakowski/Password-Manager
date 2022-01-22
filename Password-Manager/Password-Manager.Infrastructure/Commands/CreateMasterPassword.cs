namespace Password_Manager.Infrastructure.Commands
{
    public class CreateMasterPassword
    {
        public string Username { get; set; }
        public byte[] MasterPasswordHash { get; set; }
    }
}