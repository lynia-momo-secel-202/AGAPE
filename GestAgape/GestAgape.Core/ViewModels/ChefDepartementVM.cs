using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels
{
    public class ChefDepartementVM
    {

        //ChefDepartement
        [Key]
        public Guid ChefDepartementId { get; set; }
        public DateTime DateNomination { get; set; }
        public DateTime DateFin { get; set; }
        public bool? Statut { get; set; }
        public Guid DepartementId { get; set; }


        //personne

        public Guid PersonneId { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Telephone { get; set; }
        public string? StatutMatrimonial { get; set; }
        public string? Nationalite { get; set; }
        public string? Region { get; set; }
        public string? Langue { get; set; }
        public string? Handicape { get; set; }
        public string? HandicapeDes { get; set; }
        public string? Sexe { get; set; }
        public string? Photo { get; set; }
        public string? Email { get; set; }



      

    }
}
