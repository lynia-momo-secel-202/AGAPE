using GestAgape.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace GestAgape.Core.ViewModels
{
    public class RegisterVM
    {
        
        //user
        public string? UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        //personne
        public string PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        //public Guid AffectationId { get; set; }
        public string? StatutMatrimonial { get; set; }
        public string? Nationalite { get; set; }
        public string? Region { get; set; }
        public string? Langue { get; set; }
        public bool? Handicape { get; set; }
        public string? HandicapeDes { get; set; }
        public string? Sexe { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? LieuNaissance { get; set; }
        public IFormFile? CurriculumVitae { get; set; }
        //affectation
        public List<Guid>? Campus { get; set; }
        //public DateTime DateAffectation { get; set; }

        //base entity
        public DateTime? AddDate { get; set; }
        public DateTime? LatestModificationDate { get; set; }

        //role
        public _enumAppRoles[] Roles { get; set; }

        //autre
        public string? IPAdress { get; set; }
    }
}
