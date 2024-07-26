using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Scolarite
{
    public class FraisDossierExamen: BaseEntity  
    {
        #region Proprietes
        public double? Montant { get; set; }
        #endregion

        #region Relations
        public Guid ClasseId { get; set; }
        [ForeignKey(nameof(ClasseId))]
        public Classe? Classe { get; set; }
        public Guid AnneeAcademiqueId { get; set; }
        [ForeignKey(nameof(AnneeAcademiqueId))]

        public AnneeAcademique? AnneeAcademique { get; set; }
        #endregion

    }
}
