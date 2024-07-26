using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("Affectation")]
    public class Affectation : BaseEntity
    {
        #region proprietes
        public DateTime DateAffectation { get; set; }
        #endregion

        #region relation
        public Guid CampusId { get; set; }
        public virtual Campus? Campus { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUsers? User { get; set; }
        #endregion
    }
}
