using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Scolarite
{
    public class FraisMedicaux:BaseEntity
    {
        #region Proprietes
        public double? Montant { get; set; }
        #endregion

        #region Relations
        public Guid CampusId { get; set; }
        public Campus? Campus { get; set; }
        public Guid AnneeAcademiqueId { get; set; }
        public AnneeAcademique? AnneeAcademique { get; set; }
        #endregion

    }
}

