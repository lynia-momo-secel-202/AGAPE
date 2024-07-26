using FluentValidation;
using GestAgape.Core.Entities.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class ConcoursVMValidator: AbstractValidator<ConcoursVM>
    {
        public ConcoursVMValidator() {

            RuleFor(concours => concours.Libelle)
                .NotEmpty().WithMessage(FluentUtilities.LibelleDoNotEmpty)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.LibelleFormat);

            RuleFor(concours => concours.Description)
                .NotEmpty().WithMessage(FluentUtilities.LibelleDoNotEmpty);
        }
    }
}
