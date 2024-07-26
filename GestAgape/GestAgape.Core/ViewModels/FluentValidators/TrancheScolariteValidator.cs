using FluentValidation;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class TrancheScolariteValidator : AbstractValidator<TrancheScolarite>
    {
        public TrancheScolariteValidator()
        {
            RuleFor(fde => fde.ClasseId)
                .NotEmpty().WithMessage(FluentUtilities.ClasseDoNotEmpty);
            RuleFor(fde => fde.Montant)
                .NotEmpty().WithMessage(FluentUtilities.MontantDoNotEmpty);
            RuleFor(fde => fde.AnneeAcademiqueId)
                .NotEmpty().WithMessage(FluentUtilities.AnneeAcademiqueDoNotEmpty);
            RuleFor(fde => fde.CampusId)
                .NotEmpty().WithMessage(FluentUtilities.CampusDoNotEmpty2);
        }
    }
}