using FluentValidation;
using GestAgape.Core.Entities.Parametrage;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class NiveauValidator : AbstractValidator<Niveau>
    {
        public NiveauValidator()
        {
            RuleFor(niveau => niveau.Libelle)
             .NotEmpty().WithMessage(FluentUtilities.NiveauDoNotEmpty)
             .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.LibelleFormat);

        }
    }
}
