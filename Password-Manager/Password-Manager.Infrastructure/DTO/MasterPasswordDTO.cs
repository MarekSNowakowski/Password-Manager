namespace Password_Manager.Infrastructure.DTO
{
    public class MasterPasswordDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] MasterPasswordHash { get; set; }
    }
}
