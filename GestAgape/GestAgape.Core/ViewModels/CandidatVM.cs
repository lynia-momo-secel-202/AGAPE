using GestAgape.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels
{
    public class CandidatVM
    {
        //Candidat

        public Guid CandidatId { get; set; }
        public string? Code { get; set; }
        public string? NomPere { get; set; }
        public string? TelephonePere { get; set; }
        public string? TelephoneMere { get; set; }
        public string? NomMere { get; set; }
        public string? ProfessionMere { get; set; }
        public string? ProfessionPere { get; set; }
        public string? Vision { get; set; }
        public string? Quartier { get; set; }
        public string? Etablissement { get; set; }
        public string? HandicapeDes { get; set; }
        public Guid? DossierPersonnelId { get; set; }


        //Personne

        #region propriete personne
        public Guid PersonneId { get; set; }
        public string? StatutMatrimonial { get; set; }
        public string? Nationalite { get; set; }
        public string? Region { get; set; }
        public string? Langue { get; set; }
        public string? Handicape { get; set; }
        public string? Sexe { get; set; }
        public string? Photo { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public string? Email { get; set; }
        [Display(Name = "Nom(s)")]
        public string? Nom { get; set; }
        [Display(Name = "Prénom(s)")]
        public string? Prenom { get; set; }
        public string? Telephone { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? LieuNaissance { get; set; }
        public string? CurriculumVitae { get; set; }
        public IFormFile? CurriculumVitaeFile { get; set; }
        #endregion
    }
}
