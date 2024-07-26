namespace GestAgape.Core.Entities.Admission
{
    public class Concours : BaseEntity
    {
        #region Propriétés
        public string? Libelle { get; set; }
        public string? Date { get; set; }
        public string? HeureDebut { get; set; }
        public string? HeureFin { get; set; }
        public string? Description { get; set; }
        public string? Flyers { get; set; }
        public string? Resultats { get; set; }

        #endregion

        #region Relations
        public IEnumerable<DemandeAdmission>? DemandeAdmissions { get; set; }

        #endregion
    }
}
