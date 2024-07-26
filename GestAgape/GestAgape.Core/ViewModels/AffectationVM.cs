using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels
{
    public class AffectationVM
    {
        //user
        public string? UserId { get; set; }

        //affectation
        public List<Guid>? Campus { get; set; }
    }
}
