using FluentValidation;
using GestAgape.Core.Entities.Scolarite;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class FraisSoutenanceValidator : AbstractValidator<FraisSoutenance>
    {
        public FraisSoutenanceValidator()
        {
            RuleFor(fs => fs.ClasseId)
                .NotEmpty().WithMessage(FluentUtilities.ClasseDoNotEmpty);
            RuleFor(fs => fs.Montant)
                .NotEmpty().WithMessage(FluentUtilities.MontantDoNotEmpty);
            RuleFor(fs => fs.AnneeAcademiqueId)
                .NotEmpty().WithMessage(FluentUtilities.AnneeAcademiqueDoNotEmpty);
        }
    }
}
