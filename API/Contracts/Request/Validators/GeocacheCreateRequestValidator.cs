using FluentValidation;

namespace API.Contracts.Request.Validators
{
    public class GeocacheCreateRequestValidator : AbstractValidator<GeocacheCreateRequestDTO>
    {
        public GeocacheCreateRequestValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(g => g.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("{PropertyName} must be a decimal value between {From} to {To}.");

            RuleFor(g => g.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("{PropertyName} must be a decimal value between {From} to {To}.");
        }
    }
}
