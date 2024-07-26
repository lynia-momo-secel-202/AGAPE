using FluentValidation;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordVM>
    {
       
        public ResetPasswordValidator()
        {
            RuleFor(user => user.Password).NotEmpty().WithMessage(FluentUtilities.PasswordDoNotEmpty)
                                 .MinimumLength(8).WithMessage(FluentUtilities.PasswordMinLenght)
                                 .MaximumLength(16).WithMessage(FluentUtilities.PasswordMaxLenght)
                                 .Matches(@"[A-Z]+").WithMessage(FluentUtilities.PasswordMustHaveUpperCase)
                                 .Matches(@"[a-z]+").WithMessage(FluentUtilities.PasswordMustHaveLowerCase)
                                 .Matches(@"[0-9]+").WithMessage(FluentUtilities.PasswordMustHaveDigit)
                                 .Matches(@"[\!\?\*\.]+").WithMessage(FluentUtilities.PasswordMustHaveDigit);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(FluentUtilities.EmailDoNotEmpty)
                .EmailAddress().WithMessage(FluentUtilities.EmailAreInvalid)
                .Matches(FluentUtilities.RegExEmailDomain).WithMessage(FluentUtilities.EmailAreNotAllowedBecauseDomain);

            RuleFor(user => user.ConfirmPassword)
                .NotEmpty().WithMessage(FluentUtilities.PasswordDoNotEmpty)
                .Equal(user => user.Password)
                .WithMessage(FluentUtilities.PasswordDoNotMatch);


        }
    }
}
