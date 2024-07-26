using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Parametrage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GestAgape.Core.Entities.Identity
{
    [Table("Personne")]
    public class Personne : BaseEntity
    {
        #region proprietes
        public string? StatutMatrimonial { get; set; }
        public string? Nationalite { get; set; }
        public string? Region { get; set; }
        public string? Langue { get; set; }
        public string? Handicape { get; set; }
        public string? Sexe { get; set; }
        public string? Photo { get; set; }
        public string? Email { get; set; }
        [Display(Name = "Nom(s)")]
        public string? Nom { get; set; }
        [Display(Name = "Prénom(s)")]
        public string? Prenom { get; set; }
        public string? Telephone { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? LieuNaissance { get; set; }
        public string? CurriculumVitae { get; set; }
        #endregion

        #region relation
        public virtual Candidat? Candidat { get; set; }
        public virtual ChefDepartement? ChefDepartement { get; set; }
        #endregion
    }
}
