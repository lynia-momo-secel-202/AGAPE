using GestAgape.Core.Entities.Scolarite;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Admission
{
    public class Paiement : BaseEntity
    {
        #region Propriétés
        public double? Montant { get; set; }
        public virtual Motif Motif { get; set; }
        public virtual Modepaiement Modepaiement { get; set; }


        #endregion

        #region Relations
        public Guid DemandeAdmissionId { get; set; }
        [ForeignKey(nameof(DemandeAdmissionId))]
        public virtual DemandeAdmission? DemandeAdmission { get; set; }

        #endregion
    }
    public enum Motif
    {
        FraisConcours,
        FraisEtudeDossier,
        FraisScolarite,
        FraisSoutenance,
        FraisMedicaux,
        FraisInscription,
        FraisDossierExamen
    }

    public enum Modepaiement
    {
        Espece,
        OrangeMoney,
        MobileMoney,
        VersementBancaire,
        Cheque,
        Autres
    }

}
