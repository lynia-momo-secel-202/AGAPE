using FluentValidation;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class DepartementValidator : AbstractValidator<Departement>

    {
        public DepartementValidator()
        {
            RuleFor(d => d.Libelle)
                .NotEmpty().WithMessage(FluentUtilities.LibelleDoNotEmpty)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.LibelleFormat);

            RuleFor(d => d.Code)
                .NotEmpty().WithMessage(FluentUtilities.CodeDoNotEmpty);

        }
    }
}
