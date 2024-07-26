using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Parametrage
{
    public class Document : BaseEntity
    {
        #region Proprétés
        public string? Nom { get; set; }
        public string? Description { get; set; }
        #endregion

        #region Relations
        public virtual IEnumerable<DocADepose>? DocADeposes { get; set; }
        public virtual IEnumerable<Document>? Documents { get; set; }
        #endregion
    }
}
