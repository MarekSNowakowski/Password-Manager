namespace Password_Manager.Core.Domain
{
    public class Password
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Author { get; set; }
    }
}
