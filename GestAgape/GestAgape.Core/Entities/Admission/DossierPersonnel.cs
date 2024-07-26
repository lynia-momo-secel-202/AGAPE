using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Admission
{
    public class DossierPersonnel : BaseEntity
    {
        #region Proprétés
        public string? ActeNaissance { get; set; }
        public string? ReleveBac { get; set; }
        public string? CNI { get; set; }
        public string? Photos { get; set; }
        public string? ReleveNiveau1 { get; set; }
        public string? ReleveNiveau2 { get; set; }
        public string? ReleveMaster1 { get; set; }
        public string? ReleveBTS { get; set; }
        public string? ReleveLicence { get; set; }

        #endregion

        #region Relations
        public virtual Candidat? Candidat { get; set;}

        #endregion
    }
}
