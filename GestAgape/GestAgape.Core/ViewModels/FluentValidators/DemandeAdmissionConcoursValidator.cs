using FluentValidation;
using GestAgape.Core.Entities.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class DemandeAdmissionConcoursValidator : AbstractValidator<DemandeAdmission>
    {
        public DemandeAdmissionConcoursValidator()
        {
            RuleFor(da => da.AnneeAcademiqueId)
                       .NotEmpty().WithMessage(FluentUtilities.AnneeAcademiqueDoNotEmpty);

            RuleFor(da => da.ClasseId)
                       .NotEmpty().WithMessage(FluentUtilities.ClasseDoNotEmpty);

            RuleFor(da => da.ConcoursId)
                       .NotEmpty().WithMessage(FluentUtilities.ConcoursDoNotEmpty);
        }
    }
}
