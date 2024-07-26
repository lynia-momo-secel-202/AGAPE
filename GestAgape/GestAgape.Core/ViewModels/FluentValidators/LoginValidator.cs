using FluentValidation;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class LoginValidator : AbstractValidator<LoginVM>
    {
        public LoginValidator()
        {
            RuleFor(p => p.Password).NotEmpty().WithMessage(FluentUtilities.PasswordDoNotEmpty)
                                 .MinimumLength(8).WithMessage(FluentUtilities.PasswordMinLenght)
                                 .MaximumLength(16).WithMessage(FluentUtilities.PasswordMaxLenght)
                                 .Matches(@"[A-Z]+").WithMessage(FluentUtilities.PasswordMustHaveUpperCase)
                                 .Matches(@"[a-z]+").WithMessage(FluentUtilities.PasswordMustHaveLowerCase)
                                 .Matches(@"[0-9]+").WithMessage(FluentUtilities.PasswordMustHaveDigit)
                                 .Matches(@"[\!\?\*\@\$\-\.]+").WithMessage(FluentUtilities.PasswordMustHaveSpecialChar);


            RuleFor(login => login.Email)
                .NotEmpty().WithMessage(FluentUtilities.EmailDoNotEmpty)
                .EmailAddress().WithMessage(FluentUtilities.EmailAreInvalid)
                .Matches(FluentUtilities.RegExEmailDomain).WithMessage(FluentUtilities.EmailAreNotAllowedBecauseDomain);
        }
    }
}
