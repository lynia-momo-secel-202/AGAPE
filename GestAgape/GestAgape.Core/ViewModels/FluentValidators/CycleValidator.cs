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
    public class CycleValidator : AbstractValidator<Cycle>
    {
        public CycleValidator()
        {
            RuleFor(c => c.Libelle)
               .NotEmpty().WithMessage(FluentUtilities.FirstNameDoNotEmpty)
                .Matches(FluentUtilities.RegExNomPrenom).WithMessage(FluentUtilities.LibelleFormat);
            RuleFor(c => c.Code)
              .NotEmpty().WithMessage(FluentUtilities.CodeDoNotEmpty);
        }
    }

}