using CamelRegistry.Api.Dtos;
using FluentValidation;

namespace CamelRegistry.Api.Validators;

public class UpdateCamelValidator : AbstractValidator<UpdateCamelDto>
{
    public UpdateCamelValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
        RuleFor(c => c.Color).MaximumLength(15).WithMessage("Color cannot exceed 15 characters.");
        RuleFor(c => c.HumpCount).InclusiveBetween(1,2).WithMessage("Hump count must be either 1 or 2.");
        RuleFor(c => c.LastFed).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Last fed date cannot be in the future.");
    }
}
