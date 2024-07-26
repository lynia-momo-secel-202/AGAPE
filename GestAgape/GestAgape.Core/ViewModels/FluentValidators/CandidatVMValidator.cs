using FluentValidation;
using FluentValidation.Results;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class CandidatVMValidator : AbstractValidator<CandidatVM>
    {
        public CandidatVMValidator()
        {
            #region candidat
            RuleFor(candidat => candidat.NomPere)
                    .Length(2, 25).WithMessage(FluentUtilities.FirstNameLenght)
                    .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.NomFormat);

            RuleFor(candidat => candidat.TelephonePere)
                .Length(9).WithMessage(FluentUtilities.PhoneLenght)
                .Matches(FluentUtilities.RegExPhone).WithMessage(FluentUtilities.PhoneAreInvalid)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.NomFormat);

            RuleFor(candidat => candidat.NomMere)
                    .Length(2, 25).WithMessage(FluentUtilities.FirstNameLenght)
                    .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.NomFormat);

            RuleFor(candidat => candidat.TelephoneMere)
                .Length(9).WithMessage(FluentUtilities.PhoneLenght)
                .Matches(FluentUtilities.RegExPhone).WithMessage(FluentUtilities.PhoneAreInvalid);

            RuleFor(candidat => candidat.Etablissement)
                    .NotEmpty().WithMessage(FluentUtilities.EtablissementDoNotEmpty);

            RuleFor(candidat => candidat.Quartier)
                    .NotEmpty().WithMessage(FluentUtilities.ResidenceDoNotEmpty);

            #endregion

            #region personne
            RuleFor(personne => personne.Telephone)
                .NotEmpty().WithMessage(FluentUtilities.PhoneDoNotEmpty)
                .Length(9).WithMessage(FluentUtilities.PhoneLenght)
                .Matches(FluentUtilities.RegExPhone).WithMessage(FluentUtilities.PhoneAreInvalid);

            RuleFor(personne => personne.Nom)
                    .NotEmpty().WithMessage(FluentUtilities.FirstNameDoNotEmpty)
                    .Length(2, 25).WithMessage(FluentUtilities.FirstNameLenght)
                    .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.NomFormat);

            RuleFor(personne => personne.Prenom)
                    .NotEmpty().WithMessage(FluentUtilities.LastNameDoNotEmpty)
                    .Length(2, 25).WithMessage(FluentUtilities.LastNameLenght)
                    .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.PrenomFormat);

            RuleFor(personne => personne.Email)
               .NotEmpty().WithMessage(FluentUtilities.EmailDoNotEmpty)
               .EmailAddress().WithMessage(FluentUtilities.EmailAreInvalid)
               .Matches(FluentUtilities.RegExEmailDomain).WithMessage(FluentUtilities.EmailAreNotAllowedBecauseDomain);

            RuleFor(personne => personne.DateNaissance)
                    .NotEmpty().WithMessage(FluentUtilities.DateNaissanceDoNotEmpty);

            RuleFor(personne => personne.LieuNaissance)
                    .NotEmpty().WithMessage(FluentUtilities.LieuNaissanceDoNotEmpty);

            RuleFor(personne => personne.Langue)
                    .NotEmpty().WithMessage(FluentUtilities.LangueDoNotEmpty);

            RuleFor(personne => personne.StatutMatrimonial)
                    .NotEmpty().WithMessage(FluentUtilities.StatutMatrimonialDoNotEmpty);

            RuleFor(personne => personne.Nationalite)
                .NotEmpty().WithMessage(FluentUtilities.NationaliteDoNotEmpty);
            #endregion
        }
    }
}
