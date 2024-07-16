namespace Domain
{
    public class User
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

    }
}
