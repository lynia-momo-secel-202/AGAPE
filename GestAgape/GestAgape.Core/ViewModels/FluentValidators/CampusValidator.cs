using FluentValidation;
using GestAgape.Core.Entities;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class CampusValidator: AbstractValidator<Campus>
    {
        public CampusValidator()
        {
            RuleFor( c=> c.Nom)
                .NotEmpty().WithMessage(FluentUtilities.LibelleDoNotEmpty)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.LibelleFormat); 

            RuleFor(c => c.Adresse)
                .NotEmpty().WithMessage(FluentUtilities.AdresseDoNotEmpty);

            RuleFor(c => c.Responsable)
                .NotEmpty().WithMessage(FluentUtilities.ResponsableDoNotEmpty);

            RuleFor(c => c.Telephone)
                .NotEmpty().WithMessage(FluentUtilities.PhoneDoNotEmpty)
                .Length(9).WithMessage(FluentUtilities.PhoneAreInvalid)
                .Matches(FluentUtilities.RegExPhone).WithMessage(FluentUtilities.PhoneAreInvalid);

            RuleFor(c => c.IPESId)
                .NotEmpty().WithMessage(FluentUtilities.IpesDoNotEmpty);
           
        }
    }
}
