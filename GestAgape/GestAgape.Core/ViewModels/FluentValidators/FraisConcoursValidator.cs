using FluentValidation;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class FraisConcoursValidator : AbstractValidator<FraisConcours>
    {
        public FraisConcoursValidator()
        {

            RuleFor(fc => fc.Montant)
              .NotEmpty().WithMessage(FluentUtilities.MontantDoNotEmpty);
            RuleFor(fc => fc.CycleId)
                .NotEmpty().WithMessage(FluentUtilities.ClasseDoNotEmpty);
            RuleFor(fc => fc.AnneeAcademiqueId)
                .NotEmpty().WithMessage(FluentUtilities.AnneeAcademiqueDoNotEmpty);

        }
    }
}
