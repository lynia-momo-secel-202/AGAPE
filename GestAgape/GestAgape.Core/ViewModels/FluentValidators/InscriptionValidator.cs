using FluentValidation;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.Entities.Scolarite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class InscriptionValidator : AbstractValidator<Inscription>
    {
        public InscriptionValidator()
        {
        }
    }
}
