using API.Data.Interfaces;
using FluentValidation;

namespace API.Contracts.Request.Validators
{
    public class ItemCreateRequestValidator : AbstractValidator<ItemCreateRequestDTO>
    {
        public ItemCreateRequestValidator(IItemRepository itemRepository)
        {
            RuleFor(i => i.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Length(1, 50).WithMessage("{PropertyName} cannot be longer than {MaxLength} characters.")
                .Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("{PropertyName} contains invalid characters. Only letters, numbers, and spaces are allowed.")
                .MustAsync(async (name, cancellation) =>
                {
                    bool exists = await itemRepository.NameExistsAsync(name);
                    return !exists;
                }).WithMessage("The value '{PropertyValue}' is already in use. Please choose a different {PropertyName}.");

            RuleFor(i => i.ActiveStartDate)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}