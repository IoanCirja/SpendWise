namespace Domain
{
    public class UserCredentials
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword {  get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
