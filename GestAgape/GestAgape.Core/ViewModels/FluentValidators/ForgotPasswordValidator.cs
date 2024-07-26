using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordVM>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(user => user.Email)
               .NotEmpty().WithMessage(FluentUtilities.EmailDoNotEmpty)
               .EmailAddress().WithMessage(FluentUtilities.EmailAreInvalid)
               .Matches(FluentUtilities.RegExEmailDomain).WithMessage(FluentUtilities.EmailAreNotAllowedBecauseDomain);
        }
    }
}
