using GestAgape.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("ChefDepartement")]
    public class ChefDepartement: BaseEntity
    {
        #region proprietes
        public DateTime DateNomination { get; set; }
        public DateTime DateFin { get; set; }
        public bool? Statut { get; set; }
        #endregion

        #region relation
        public Guid PersonneId { get; set; }
        [ForeignKey(nameof(PersonneId))]
        public virtual Personne? Personne { get; set; }
        public Guid DepartementId { get; set; }
        [ForeignKey(nameof(DepartementId))]
        public virtual Departement? Departement { get; set; }

        #endregion
    }
}
