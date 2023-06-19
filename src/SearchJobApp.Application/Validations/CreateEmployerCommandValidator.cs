using System.Text.RegularExpressions;
using FluentValidation;
using SearchJobApp.Application.Commands;

namespace SearchJobApp.Application.Validations;

public class CreateEmployerCommandValidator : AbstractValidator<CreateEmployerCommand>
{
    public CreateEmployerCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotNull().NotEmpty().WithMessage("Email field is required.")
            .Must(IsValidEmailAddress).WithMessage("Invalid email address. Example: name@hostname.com");

        RuleFor(c => c.Password)
            .NotNull().NotEmpty().WithMessage("Password field is required.");

        RuleFor(c => c.Phone)
            .NotEmpty().NotNull().WithMessage("Phone field is required.")
            .Must(IsValidPhoneNumber).WithMessage("Invalid phone number. Example: 905555555555");

        RuleFor(c => c.Title)
            .NotNull().NotEmpty().WithMessage("Title field is required.");

        RuleFor(c => c.Address)
            .NotNull().NotEmpty().WithMessage("Address field is required.");
    }

    private bool IsValidEmailAddress(string email) => Regex.IsMatch(email,
        @"[-A-Za-z0-9!#$%&'*+/=?^_`{|}~]+(?:\.[-A-Za-z0-9!#$%&'*+/=?^_`{|}~]+)*@(?:[A-Za-z0-9](?:[-A-Za-z0-9]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[-A-Za-z0-9]*[A-Za-z0-9])?");

    private bool IsValidPhoneNumber(string phone) => Regex.IsMatch(phone, @"90\d\d\d\d\d\d\d\d\d\d");
}