namespace GestAgape.Core.ViewModels
{
    public class LoginVM
    {
        public string? UserId { get;set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? IPAdress { get; set; }
        public Guid CampusId { get; set; }
    }
}
