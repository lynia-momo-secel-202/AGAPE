using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.Entities.Scolarite;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Admission
{
    public class DemandeAdmission : BaseEntity
    {
        #region Propriétés
        public virtual Decision Decision { get; set; }
        public string? Mention { get; set; }
        public virtual TypeAdmission TypeAdmission { get; set; }

        #endregion

        #region Relations
        public Guid CandidatId {get; set;}
        [ForeignKey(nameof(CandidatId))]
        public virtual Candidat? Candidat { get; set; }
        public Guid? ConcoursId { get; set;}
        [ForeignKey(nameof(ConcoursId))]
        public virtual Concours? Concours { get; set; }  
        public Guid AnneeAcademiqueId { get; set;}
        [ForeignKey(nameof(AnneeAcademiqueId))]
        public virtual AnneeAcademique? AnneeAcademique { get; set; }
        public Guid ClasseId { get; set;}
        [ForeignKey(nameof(ClasseId))]
        public Classe? Classe { get; set; } 
        public Guid? InscriptionId { get; set;}
        [ForeignKey(nameof(InscriptionId))]
        public Inscription? Inscription { get; set; }
        public IEnumerable<Paiement>? Paiements { get; set; }

        #endregion
    }
    public enum TypeAdmission
    {
        Concours,
        EtudeDossier
    }
    public enum Decision
    {
        EnCours,
        Admis,
        Refuse
    }
}
