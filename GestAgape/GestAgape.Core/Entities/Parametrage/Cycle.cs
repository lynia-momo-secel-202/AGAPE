using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("Cycle")]
    public class Cycle : BaseEntity
    {
        #region proprietes

        [Display(Name = "Libellé du cycle")]
        public string? Libelle { get; set; }
        public string? Code { get; set; }

        #endregion

        #region relation
        public virtual IEnumerable<Classe>? Classes { get; set; }
        #endregion

    }
}
