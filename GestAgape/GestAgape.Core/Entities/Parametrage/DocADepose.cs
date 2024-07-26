using System.ComponentModel.DataAnnotations.Schema;

namespace GestAgape.Core.Entities.Parametrage
{
    public class DocADepose : BaseEntity
    {
        #region Proprétés
        #endregion

        #region Relations
        public Guid ClasseId { get; set; }
        [ForeignKey(nameof(ClasseId))]
        public virtual Classe? Classe { get; set; }

        public Guid DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public virtual Document? Document { get; set; }
        #endregion
    }
}
