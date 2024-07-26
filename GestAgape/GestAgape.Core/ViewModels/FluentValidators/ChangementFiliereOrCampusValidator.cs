using FluentValidation;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class ChangementFiliereOrCampusValidator : AbstractValidator<ChangementFiliereOrCampus>
    {
        public ChangementFiliereOrCampusValidator()
        {
            RuleFor(c => c.NextFiliere)
          .NotEmpty().WithMessage(FluentUtilities.SerieDoNotEmpty);
            RuleFor(c => c.MotifChangement)
             .NotEmpty().WithMessage(FluentUtilities.MotifDoNotEmpty);
        }
    }
}
