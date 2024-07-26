using FluentValidation;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class FraisDossierExamenValidator : AbstractValidator<FraisDossierExamen>
    {
        public FraisDossierExamenValidator()
        {
            RuleFor(fde => fde.ClasseId)
                .NotEmpty().WithMessage(FluentUtilities.ClasseDoNotEmpty);
            RuleFor(fde => fde.Montant)
                .NotEmpty().WithMessage(FluentUtilities.FraisDossierExamenDoNotEmpty);
            RuleFor(fde => fde.AnneeAcademiqueId)
                .NotEmpty().WithMessage(FluentUtilities.AnneeAcademiqueDoNotEmpty);
        }
    }
}
