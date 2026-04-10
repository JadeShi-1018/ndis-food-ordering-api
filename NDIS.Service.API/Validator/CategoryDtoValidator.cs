using FluentValidation;
using NDIS.Service.API.DTOs.Category;

namespace NDIS.Service.API.Validator
{
    public class CategoryDtoValidator : AbstractValidator<CategoryBaseDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100);

            RuleFor(x => x.CategoryDescription)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.CategoryDescription));
        }
    }
}
