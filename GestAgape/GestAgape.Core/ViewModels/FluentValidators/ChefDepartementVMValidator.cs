using FluentValidation;
using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class ChefDepartementVMValidator : AbstractValidator<ChefDepartementVM>
    {

        #region 

        public ChefDepartementVMValidator()
        {

            RuleFor(personne => personne.Nom)
                .NotEmpty().WithMessage(FluentUtilities.FirstNameDoNotEmpty)
                .Length(2,15).WithMessage(FluentUtilities.FirstNameLenght)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.NomFormat);

            RuleFor(personne => personne.Prenom)
                .NotEmpty().WithMessage(FluentUtilities.LastNameDoNotEmpty)
                .Length(2, 15).WithMessage(FluentUtilities.LastNameLenght)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.PrenomFormat);


            RuleFor(personne => personne.Telephone)
                .NotEmpty().WithMessage(FluentUtilities.PhoneDoNotEmpty)
                .Length(9).WithMessage(FluentUtilities.PhoneLenght)
                .Matches(FluentUtilities.RegExPhone).WithMessage(FluentUtilities.PhoneAreInvalid);

            RuleFor(personne => personne.Email)
                .NotEmpty().WithMessage(FluentUtilities.EmailDoNotEmpty)
                .EmailAddress().WithMessage(FluentUtilities.EmailAreInvalid)
                .Matches(FluentUtilities.RegExEmailDomain).WithMessage(FluentUtilities.EmailAreInvalid);

            RuleFor(personne => personne.Nationalite)
                .NotEmpty().WithMessage(FluentUtilities.NationaliteDoNotEmpty);

            RuleFor(personne => personne.Langue)
                .NotEmpty().WithMessage(FluentUtilities.LangueDoNotEmpty);

            RuleFor(personne => personne.StatutMatrimonial)
                .NotEmpty().WithMessage(FluentUtilities.StatutMatrimonialDoNotEmpty);

            RuleFor(personne => personne.Sexe)
                .NotEmpty().WithMessage(FluentUtilities.SexeDoNotEmpty);

            RuleFor(personne => personne.DepartementId)
               .NotEmpty().WithMessage(FluentUtilities.DepartementDoNotEmpty);

        }


        #endregion

    }
}
