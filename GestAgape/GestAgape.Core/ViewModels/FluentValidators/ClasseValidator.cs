using FluentValidation;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class ClasseValidator: AbstractValidator<Classe>

    {
        public ClasseValidator()
        {
            RuleFor(c => c.Cycle)
                 .NotEmpty().WithMessage(FluentUtilities.CycleDoNotEmpty);


            RuleFor(c => c.Niveau)
                 .NotEmpty().WithMessage(FluentUtilities.NiveauDoNotEmpty);

            RuleFor(c => c.Filiere)
                 .NotEmpty().WithMessage(FluentUtilities.SerieDoNotEmpty);

        }
    }
}
