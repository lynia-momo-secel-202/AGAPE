namespace GestAgape.Core.Entities.Scolarite
{
    public class ChangementFiliereOrCampus : BaseEntity
    {
        #region Proprietes
        public string? PastFiliere { get; set; }
        public string? NextFiliere { get; set; }
        public string? PastCampus { get; set; }
        public string? NextCampus { get; set; }
        public string? MotifChangement { get; set; }
        public Statut Statut { get; set; }

        #endregion

        #region Relations
        public Guid InscriptionId { get; set; }
        public Inscription? Inscription { get; set; }

        #endregion
    }

    public enum Statut
    {
        EnAttente,
        accepte,
        Refuse
    }
}
