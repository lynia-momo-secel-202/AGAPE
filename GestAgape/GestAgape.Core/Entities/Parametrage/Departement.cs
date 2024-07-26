using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    [Table("Departement")]
    public class Departement:BaseEntity
    {
        #region proprietes
        public string? Libelle { get; set; }
        public string? Code { get; set; }
        #endregion

        #region relations
        public virtual IEnumerable<ChefDepartement>? ChefDepartements { get; set; }
        public virtual IEnumerable<Filiere>? Filieres { get; set; }

        #endregion
    }
}
