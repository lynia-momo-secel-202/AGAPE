using FluentValidation;
using GestAgape.Core.Entities.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class DemandeAdmissionValidator: AbstractValidator<DemandeAdmission>
    {
        public DemandeAdmissionValidator() {
            RuleFor(da => da.AnneeAcademiqueId)
                       .NotEmpty().WithMessage(FluentUtilities.AnneeAcademiqueDoNotEmpty);

            RuleFor(da => da.ClasseId)
                       .NotEmpty().WithMessage(FluentUtilities.ClasseDoNotEmpty);


        }
    }
}
