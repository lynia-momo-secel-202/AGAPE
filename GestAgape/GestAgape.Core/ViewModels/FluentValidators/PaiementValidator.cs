using FluentValidation;
using GestAgape.Core.Entities.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class PaiementValidator: AbstractValidator<Paiement>
    {
        public PaiementValidator() {
            RuleFor(paiement => paiement.Montant)
                   .NotEmpty().WithMessage(FluentUtilities.PaiementDoNotEmpty);
        }
    }
}
