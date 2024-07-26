using FluentValidation;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class FraisInscriptionValidator : AbstractValidator<FraisInscription>
    {
        public FraisInscriptionValidator()
        {
        }
    }
}
