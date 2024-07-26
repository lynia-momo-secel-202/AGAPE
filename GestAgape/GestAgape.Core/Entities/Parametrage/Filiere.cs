using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("Filiere")]
    public class Filiere : BaseEntity
    {

        #region proprietes
        [Display(Name = "Libellé")]
        public string? Libelle{ get; set; }
        public string? Code { get; set; }
        #endregion

        #region relation
        public Guid DepartementID { get; set; }
        [ForeignKey(nameof(DepartementID))]
        public virtual Departement? Departement { get; set; }
        public virtual IEnumerable<Classe>? Classes { get; set; }
        #endregion
    }
}
