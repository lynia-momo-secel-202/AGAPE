using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Scolarite
{
    public class Inscription:BaseEntity
    {
        #region Relations
        public virtual DemandeAdmission DemandeAdmission { get; set; }
        public Guid MatriculeId { get; set; }
        public Matricule? Matricule { get; set; }
        public Guid CampusId { get; set; }
        public Campus? Campus { get; set; }
        public IEnumerable<Bourse>? Bourses { get; set; }
        public IEnumerable<ChangementFiliereOrCampus>? Changements { get; set; }
        #endregion

    }
}   
 