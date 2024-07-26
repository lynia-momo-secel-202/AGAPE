using FluentValidation;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class AffectationVMValidator : AbstractValidator<AffectationVM>
    {
        public AffectationVMValidator()
        {
            #region user
            RuleFor(user => user.UserId)
                       .NotEmpty().WithMessage(FluentUtilities.UserDoNotEmpty);
            #endregion
            #region campus
            RuleFor(campus => campus.Campus)
                       .NotEmpty().WithMessage(FluentUtilities.CampusDoNotEmpty);
            #endregion
        }
    }
}
