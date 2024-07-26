using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("Campus")]
    public class Campus : BaseEntity
    {
        #region proprietes
        public string? Nom { get; set; }
        public string? Adresse { get; set; }
        public string? Responsable { get; set; }
        public string? Telephone { get; set; }
        #endregion

        #region relation
        [Display(Name = "Ecole")]
        public Guid IPESId { get; set; }
        [ForeignKey(nameof(IPESId))]
        public Ipes? Ipes { get; set; }

        public virtual IEnumerable<Affectation>? Affectations { get; set; }

        #endregion
    }
}
