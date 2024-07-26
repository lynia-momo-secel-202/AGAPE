using FluentValidation;
using GestAgape.Core.Entities.Parametrage;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class FiliereValidator : AbstractValidator<Filiere>
    {
        public FiliereValidator()
        {
            RuleFor(f => f.Libelle)
               .NotEmpty().WithMessage(FluentUtilities.FirstNameDoNotEmpty)
            .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.LibelleFormat);
            RuleFor(c =>c .Code)
                .NotEmpty().WithMessage(FluentUtilities.CodeDoNotEmpty);
        }
    }
}