using GestAgape.Core.Entities.Parametrage;
using Microsoft.AspNetCore.Identity;


namespace GestAgape.Core.Entities
{
    public class ApplicationUsers : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime RegistredDate { get; set; }
        public DateTime LatestModificationDate { get; set; }
        public string? ProfilePicture { get; set; }
        public ICollection<Connexion>? Connexions { get; set; }
        public ICollection<PasswordHistory>? PasswordHistories { get; set; }

        public virtual IEnumerable<Affectation>? Affectations { get; set; }

    }
}
