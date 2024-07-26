using GestAgape.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Admission
{
    public class Candidat :BaseEntity
    {
        #region Propriétés
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
        #endregion

        #region Relations
        public IEnumerable<DemandeAdmission>? Demandes { get; set; }
        public Guid PersonneId { get; set; }
        [ForeignKey(nameof(PersonneId))]
        public virtual Personne? Personne { get; set; }
        public Guid? DossierPersonnelId { get; set; }
        [ForeignKey(nameof(DossierPersonnelId))]
        public virtual DossierPersonnel? DossierPersonnel { get; set; }
        #endregion
    }
}
