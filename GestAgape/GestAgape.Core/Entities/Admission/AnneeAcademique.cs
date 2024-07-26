namespace GestAgape.Core.Entities.Admission
{
    public class AnneeAcademique : BaseEntity
    {
        #region Propriétés
        public string? AnneeDebut { get; set; }
        public string? AnneeFin { get; set; }
        #endregion

        #region Relations
        public IEnumerable<DemandeAdmission>? Demandes { get; set; }
        #endregion
    }
}
