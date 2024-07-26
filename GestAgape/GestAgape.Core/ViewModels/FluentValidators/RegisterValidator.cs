using FluentValidation;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class RegisterValidator : AbstractValidator<RegisterVM>
    {
       
        public RegisterValidator()
        {
            RuleFor(user => user.Password).NotEmpty().WithMessage(FluentUtilities.PasswordDoNotEmpty)
                                 .MinimumLength(8).WithMessage(FluentUtilities.PasswordMinLenght)
                                 .MaximumLength(16).WithMessage(FluentUtilities.PasswordMaxLenght)
                                 .Matches(@"[A-Z]+").WithMessage(FluentUtilities.PasswordMustHaveUpperCase)
                                 .Matches(@"[a-z]+").WithMessage(FluentUtilities.PasswordMustHaveLowerCase)
                                 .Matches(@"[0-9]+").WithMessage(FluentUtilities.PasswordMustHaveDigit)
                                 .Matches(@"[\!\?\*\-\.]+").WithMessage(FluentUtilities.PasswordMustHaveSpecialChar);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(FluentUtilities.EmailDoNotEmpty)
                .EmailAddress().WithMessage(FluentUtilities.EmailAreInvalid)
                .Matches(FluentUtilities.RegExEmailDomain).WithMessage(FluentUtilities.EmailAreNotAllowedBecauseDomain);

            RuleFor(user => user.ConfirmedPassword)
                .NotEmpty().WithMessage(FluentUtilities.PasswordDoNotEmpty)
                .Equal(user => user.Password)
                .WithMessage(FluentUtilities.PasswordDoNotMatch);

            RuleFor(user => user.PhoneNumber)
                .NotEmpty().WithMessage(FluentUtilities.PhoneDoNotEmpty)
                .Length(9).WithMessage(FluentUtilities.PhoneLenght)
                .Matches(FluentUtilities.RegExPhone).WithMessage(FluentUtilities.PhoneAreInvalid);

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage(FluentUtilities.FirstNameDoNotEmpty)
                .Length(2, 25).WithMessage(FluentUtilities.FirstNameLenght);

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage(FluentUtilities.LastNameDoNotEmpty)
                .Length(2, 25).WithMessage(FluentUtilities.LastNameLenght);

        }
    }
}
