using FluentValidation;
using GestAgape.Core.Entities.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class AnneeAcademiqueValidator: AbstractValidator<AnneeAcademique>
    {
        public AnneeAcademiqueValidator()
        {
            RuleFor(annee => annee.AnneeDebut)
                .NotEmpty().WithMessage(FluentUtilities.AnneeDebutDoNotEmpty)
                .Length(4).WithMessage(FluentUtilities.AnneeLenght);
        }
    }
}
