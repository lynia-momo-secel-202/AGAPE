﻿using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Admission
{
    public class FraisEtudeDossier: BaseEntity
    {
        #region Proprietes
        public double? Montant { get; set; }
        #endregion

        #region Relations
        public Guid ClasseId { get; set; }
        public Classe? Classe { get; set; }
        public Guid AnneeAcademiqueId { get; set; }
        public AnneeAcademique? AnneeAcademique { get; set; }

        #endregion

    }
}

