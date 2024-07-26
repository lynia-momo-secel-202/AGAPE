using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("Niveau")]
    public class Niveau : BaseEntity
    {
        #region proprietes
        [Required]
        [Display(Name = "Libellé du niveau")]
        public string? Libelle { get; set; }
        #endregion
        #region relation
        public virtual IEnumerable<Classe>? Classes { get; set; }

        #endregion
    }
}
