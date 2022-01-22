namespace Password_Manager.WebApp.Models
{
    public class MasterPasswordVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] MasterPasswordHash { get; set; }
    }
}
