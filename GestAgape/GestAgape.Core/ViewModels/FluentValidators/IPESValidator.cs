using FluentValidation;
using GestAgape.Core.Entities.Parametrage;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class IPESValidator : AbstractValidator<Ipes>
    {
        public IPESValidator()
        {
            RuleFor(c => c.Nom)
                .NotEmpty().WithMessage(FluentUtilities.NomIPESDoNotEmpty)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.LibelleFormat);

            RuleFor(c => c.AdresseCampusPrincipal)
          .NotEmpty().WithMessage(FluentUtilities.AdresseDoNotEmpty);
            RuleFor(c => c.SiteWeb)
          .NotEmpty().WithMessage(FluentUtilities.SiteWebDoNotEmpty);
            RuleFor(c => c.BoitePostale)
          .NotEmpty().WithMessage(FluentUtilities.BoitePostaleIPESDoNotEmpty);
            RuleFor(c => c.Telephone)
          .NotEmpty().WithMessage(FluentUtilities.ContactDoNotEmpty)
             .NotEmpty().WithMessage(FluentUtilities.PhoneDoNotEmpty)
             .Length(9).WithMessage(FluentUtilities.PhoneLenght)
             .Matches(FluentUtilities.RegExPhone).WithMessage(FluentUtilities.PhoneAreInvalid);
        //    RuleFor(l => l.Logo)
        //        .NotEmpty().WithMessage(FluentUtilities.LogoDoNotEmpty);
        }
    }
}
