using FluentValidation;
using FluentValidation.Results;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class UpdateDAFiliereValidator : AbstractValidator<ChangementFiliereOrCampus>
    {
        public UpdateDAFiliereValidator() {
                RuleFor(c => c.NextFiliere)
              .NotEmpty().WithMessage(FluentUtilities.SerieDoNotEmpty);
        }

       
    }
}
