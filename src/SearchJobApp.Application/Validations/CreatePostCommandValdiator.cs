using FluentValidation;
using SearchJobApp.Application.Commands;

namespace SearchJobApp.Application.Validations;

public class CreatePostCommandValdiator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValdiator()
    {
        RuleFor(c => c.EmployerId)
            .NotNull().NotEmpty().WithMessage("EmployerId field is required.");

        RuleFor(c => c.Title)
            .NotNull().NotEmpty().WithMessage("Title field is required.");

        RuleFor(c => c.Message)
            .NotNull().NotEmpty().WithMessage("Message field is required.");
    }
}