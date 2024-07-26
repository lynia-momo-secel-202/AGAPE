using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Scolarite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("Classe")]
    public class Classe : BaseEntity
    {
        #region proprietes
        [Display(Name = "Libellé")]
        public string? Libelle { get; set; }
        public string? Code { get; set; }
        public string? ProgrammeAcademique { get; set; }

        #endregion

        #region relation
        [Display(Name = "Cycle")]
        public Guid CycleId { get; set; }
        [ForeignKey(nameof(CycleId))]
        public virtual Cycle? Cycle { get; set; }

        [Display(Name = "Filiere")]
        public Guid FiliereId { get; set; }
        public Filiere? Filiere { get; set; }


        [Display(Name = "Niveau")]
        public Guid NiveauId { get; set; }
        public Niveau? Niveau { get; set; }

        public virtual IEnumerable<DemandeAdmission>? DemandeAdmissions { get; set; }

        public virtual IEnumerable<FraisInscription> FraisInscriptions { get; set; }
        public virtual IEnumerable<DocADepose>? DocADeposes { get; set; }

        #endregion
    }
}
