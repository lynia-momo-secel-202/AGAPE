using FluentValidation;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class ChangementFiliereOrCampusValidator2 : AbstractValidator<ChangementFiliereOrCampus>
    {
        public ChangementFiliereOrCampusValidator2()
        {
            RuleFor(c => c.MotifChangement)
          .NotEmpty().WithMessage(FluentUtilities.MotifDoNotEmpty);

        }
    }
}
