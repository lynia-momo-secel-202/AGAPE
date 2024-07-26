using GestAgape.Core.Entities.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Scolarite
{
    public class Bourse:BaseEntity
    {
        #region Proprietes
        public double? Montant { get; set; }
        public string? MotifReduction { get; set; }
        public Statut Statut { get; set; }
        #endregion

        #region Relation
        public Guid InscriptionId { get; set; }
        public Inscription? Inscription { get; set; }
        #endregion
    }
}
