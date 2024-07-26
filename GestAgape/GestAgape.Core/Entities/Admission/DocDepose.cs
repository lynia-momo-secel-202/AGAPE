using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Admission
{
    public class DocDepose : BaseEntity
    {
        #region Proprétés
        /// <summary>
        /// si oui ou non le fichier a ete remi par le candidat 
        /// </summary>
        public bool Depose { get; set; }
        /// <summary>
        /// souce du document
        /// </summary>
        public string? Docpath { get; set; }
        /// <summary>
        /// date a la quelle le document est scane
        /// </summary>
        public DateTime? DateImp { get; set; }
        #endregion

        #region Relations
        /// <summary>
        /// cle etrangere du candidat
        /// </summary>
        public Guid CandidatId { get; set; }
        [ForeignKey(nameof(CandidatId))]
        public virtual Candidat? Candidat { get; set; }

        /// <summary>
        /// cle etrangere du document 
        /// </summary>
        public Guid DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public virtual Document? Document { get; set; }
        #endregion
    }
}
