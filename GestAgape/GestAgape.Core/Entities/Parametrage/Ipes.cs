using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("IPES")]
    public class Ipes : BaseEntity
    {
        #region Proprietes

        [Display(Name = "Libellé")]
        public string? Nom { get; set; }
        public string? AdresseCampusPrincipal { get; set; }
        public string? SiteWeb { get; set; }
        public string? BoitePostale { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Fax { get; set; }
        public string? Logo { get; set; }
        public string? Cachet { get; set; }
        public string? NumeroCompteBancaire { get; set; }

        #endregion

        #region relation
        public virtual IEnumerable<Campus>? Campus { get; set; }

        #endregion
    }
}
