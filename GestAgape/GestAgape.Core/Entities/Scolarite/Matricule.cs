using GestAgape.Core.Entities.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Scolarite
{
    public class Matricule:BaseEntity
    {
    #region Proprietes
    public string? LibelleMatricule { get; set; }

    #endregion

    #region Relations
    public IEnumerable<Inscription>? Inscription { get; set; }

    #endregion
    }
}
